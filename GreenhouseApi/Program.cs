using System.Text;
using Domain.IClients;
using Domain.IRepositories;
using Domain.IServices;
using EFCGreenhouse;
using EFCGreenhouse.Repositories;
using GreenhouseApi.Middleware;
using GreenhouseService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ML_Model;

namespace GreenhouseApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Greenhouse API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        builder.Services.AddControllers();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins(
                        "http://localhost:5173",
                        "https://sep4-greenhouse.github.io"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        var connectionString = Environment.GetEnvironmentVariable("AIVEN_DB_CONNECTION");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Database connection string not found. Set the AIVEN_DB_CONNECTION environment variable.");
        }
        builder.Services.AddDbContext<GreenhouseDbContext>(options =>
            options.UseNpgsql(connectionString)
        );

        builder.Services.AddHttpClient<IMlHttpClient, MlHttpClient>(client =>
        {
            var mlBaseUrl = builder.Configuration["MLService:BaseUrl"] 
                            ?? throw new InvalidOperationException("MLService BaseUrl missing");
            client.BaseAddress = new Uri(mlBaseUrl);
        });

        // Repositories
        builder.Services.AddScoped<IActuatorActionRepository, ActuatorActionRepository>();
        builder.Services.AddScoped<IActuatorRepository, ActuatorRepository>();
        builder.Services.AddScoped<IAlertRepository, AlertRepository>();
        builder.Services.AddScoped<IGreenhouseRepository, GreenhouseRepository>();
        builder.Services.AddScoped<IPlantRepository, PlantRepository>();
        builder.Services.AddScoped<IPredictionLogRepository, PredictionLogRepository>();
        builder.Services.AddScoped<ISensorReadingRepository, SensorReadingRepository>();
        builder.Services.AddScoped<ISensorRepository, SensorRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IThresholdRepository, ThresholdRepository>();

        // Services
        builder.Services.AddScoped<IActuatorService, ActuatorService>();
        builder.Services.AddScoped<IAlertService, AlertService>();
        builder.Services.AddScoped<IGreenhouseService, GreenhouseService.Services.GreenhouseService>();
        builder.Services.AddScoped<ISensorService, SensorService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IMlModelService, MlModelService>();

        var config = builder.Configuration;
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!))
                };
                
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                        logger.LogError("Authentication failed: {Error}", context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                        logger.LogInformation("Token received: {Token}", context.Token ?? "no token");
                        return Task.CompletedTask;
                    }
                };
            });

        builder.Services.AddAuthorization();
        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Greenhouse API V1");
            c.RoutePrefix = string.Empty;
        });

        app.UseGlobalExceptionHandler();

        app.UseCors("AllowFrontend");

        app.Use(async (context, next) =>
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Processing request: {Method} {Path}", context.Request.Method, context.Request.Path);

            await next();

            logger.LogInformation("Finished processing request: {Method} {Path}", context.Request.Method, context.Request.Path);
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        var port = Environment.GetEnvironmentVariable("PORT") ?? "5001";
        app.Run($"http://0.0.0.0:{port}");
    }
}