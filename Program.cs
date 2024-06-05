using dotenv.net;
using Microsoft.EntityFrameworkCore;
using DotnetNBA.Data;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();