using Domain.Interfaces;
using Domain.IServices;
using EFCGreenhouse.Repositories;
using EFCGreenhouse;
using GreenhouseApi.Services;
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

// 🔹 Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")  // for the frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

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

app.UseRouting();     // 🔹 (IMPORTANT for CORS!)

app.UseCors();        // 🔹 (Enable CORS AFTER routing)

app.UseAuthorization();

// 🔹 Map controllers
app.MapControllers();

app.Run();

// (WeatherForecast record is not needed for your app, but it's okay to leave it)
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
