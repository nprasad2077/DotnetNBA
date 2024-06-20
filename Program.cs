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

// HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
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

// Enable CORS
app.UseCors("AllowAllOrigins");

// Use HTTPS Redirection and configure Kestrel with a certificate only in Production
if (app.Environment.IsProduction())
{
    var pfxFilePath = Environment.GetEnvironmentVariable("PFX_FILE_PATH");
    var pfxPassword = Environment.GetEnvironmentVariable("PFX_PASSWORD");
    app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyAPI v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });

    

    if (!string.IsNullOrEmpty(pfxFilePath) && !string.IsNullOrEmpty(pfxPassword))
    {
        var certificate = new X509Certificate2(pfxFilePath, pfxPassword);

        app.UseHttpsRedirection();
        app.Urls.Add("https://*:7012");
        app.Urls.Add("http://*:5214");
    }
    else
    {
        app.Logger.LogWarning("HTTPS is configured in production but no certificate was provided.");
    }
}
else
{
    // If not in production, configure HTTP only
    app.Urls.Add("http://*:5214");
}

app.UseAuthorization();

app.MapControllers();

app.Run();
