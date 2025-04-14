using Domain.Interfaces;
using EFCGreenhouse.Repositories;
using MLModelClient.Services;

using EFCGreenhouse;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// 🔹 Register services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔹 Register your controller dependencies
builder.Services.AddScoped<IMlModelService, MlModelService>();

// 🔹 Register controllers support
builder.Services.AddControllers();

// 🔹 Register the ML model service
builder.Services.AddHttpClient<IMlModelService, MlModelService>();

// 🔹 Register the EF Core DbContext
builder.Services.AddScoped<IPredictionLogRepository, PredictionLogRepository>();

// 🔹 Register the DbContext with SQL
builder.Services.AddDbContext<GreenhouseDbContext>(options =>
    options.UseSqlite("Data Source=greenhouse.db"));




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Greenhouse API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

// 🔹 Map controllers
app.MapControllers();  // 👈 This is MISSING in your code






app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}