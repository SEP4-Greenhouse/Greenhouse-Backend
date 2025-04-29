using Domain.IRepositories;
using Domain.IServices;
using EFCGreenhouse.Repositories;
using EFCGreenhouse;
using GreenhouseService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

// 🔹 Add CORS policy for frontend at localhost:5173
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")  // for the frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 🔹 Add logging
builder.Services.AddLogging();

var app = builder.Build();

// 🔹 Add diagnostics and logging
// 🔹 Add diagnostics and logging
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Greenhouse API V1");
    c.RoutePrefix = string.Empty; // Access Swagger at the root URL
});

// 🔹 Enable CORS (IMPORTANT - Enable CORS after routing)
app.UseCors();       

// 🔹 Middleware to log request and response data
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Processing request: {Method} {Path}", context.Request.Method, context.Request.Path);
    
    await next();

    logger.LogInformation("Finished processing request: {Method} {Path}", context.Request.Method, context.Request.Path);
});

app.UseRouting();     

// 🔹 Enable authorization (if needed)
app.UseAuthorization();

// 🔹 Map controllers
app.MapControllers();
app.Run("http://0.0.0.0:5001"); 
