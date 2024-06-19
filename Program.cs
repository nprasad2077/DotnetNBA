using dotenv.net;
using Microsoft.EntityFrameworkCore;
using DotnetNBA.Data;
using System.Text.RegularExpressions;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables
DotEnv.Load();

// Read the configuration and replace placeholders
var configuration = builder.Configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
configuration = Regex.Replace(configuration, @"\$\{(.*?)\}", match =>
{
    var envVar = match.Groups[1].Value;
    return Environment.GetEnvironmentVariable(envVar) ?? match.Value;
});

// Override the connection string in the configuration
builder.Configuration["ConnectionStrings:DefaultConnection"] = configuration;

builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // HTTP request pipeline for development.
    app.UseDeveloperExceptionPage();
    app.UseCors("AllowAllOrigins");

    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyAPI v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}
else if (app.Environment.IsProduction())
{
    var pfxFilePath = Environment.GetEnvironmentVariable("PFX_FILE_PATH");
    var pfxPassword = Environment.GetEnvironmentVariable("PFX_PASSWORD");

    if (!string.IsNullOrEmpty(pfxFilePath) && !string.IsNullOrEmpty(pfxPassword))
    {
        var certificate = new X509Certificate2(pfxFilePath, pfxPassword);

        app.UseHttpsRedirection();

        // Configure Kestrel to use HTTPS in production.
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(7012, listenOptions =>
            {
                listenOptions.UseHttps(certificate);
            });
            options.ListenAnyIP(5214);
        });

        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        // specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyAPI v1");
            c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
        });
    }
    else
    {
        app.Logger.LogWarning("HTTPS is configured in production but no certificate was provided.");
    }
}

// Enable CORS
app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
