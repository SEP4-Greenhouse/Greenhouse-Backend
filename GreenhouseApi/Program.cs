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

//builder.Services.AddScoped<IMlModelService, MlModelService>();
builder.Services.AddScoped<IPredictionLogRepository, PredictionLogRepository>();
builder.Services.AddScoped<IActuatorActionRepository, ActuatorActionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<ISensorReadingRepository, SensorReadingRepository>();
builder.Services.AddScoped<ISensorService, SensorService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGreenhouseService, GreenhouseService.Services.GreenhouseService>();
builder.Services.AddScoped<IGreenhouseRepository, GreenhouseRepository>();
builder.Services.AddScoped<IActuatorRepository, ActuatorRepository>();
builder.Services.AddScoped<IAlertService, AlertService.Services.AlertService>();
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<IPlantRepository, PlantRepository>();
builder.Services.AddScoped<IActuatorService, ActuatorService>();


builder.Services.AddHttpClient<IMlHttpClient, MlHttpClient>(client =>
{
    client.BaseAddress = new Uri("http://host.docker.internal:8000");
});

builder.Services.AddDbContext<GreenhouseDbContext>(options =>
{
    var connectionString = Environment.GetEnvironmentVariable("AIVEN_DB_CONNECTION");
    options.UseNpgsql(connectionString);
});
builder.Services.AddControllers()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
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