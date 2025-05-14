using Domain.IClients;
using Domain.IRepositories;
using Domain.IServices;
using EFCGreenhouse.Repositories;
using EFCGreenhouse;
using GreenhouseService.Services;
using Microsoft.EntityFrameworkCore;
using ML_Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddScoped<IMlModelService, MlModelService>();
builder.Services.AddScoped<IPredictionLogRepository, PredictionLogRepository>();
builder.Services.AddScoped<IActionRepository, ActionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<ISensorDataService, SensorDataService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddHttpClient<IMlHttpClient, MlHttpClient>(client =>
{
    client.BaseAddress = new Uri("http://host.docker.internal:8000");
});

builder.Services.AddDbContext<GreenhouseDbContext>(options =>
{
    var connectionString = Environment.GetEnvironmentVariable("AIVEN_DB_CONNECTION");
    options.UseNpgsql(connectionString);
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddLogging();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Greenhouse API V1");
    c.RoutePrefix = string.Empty;
});

app.UseCors();

app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Processing request: {Method} {Path}", context.Request.Method, context.Request.Path);

    await next();

    logger.LogInformation("Finished processing request: {Method} {Path}", context.Request.Method, context.Request.Path);
});

app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run("http://0.0.0.0:5001");
//app.Run();