using System.Net.Http.Json;
using Domain.DTOs;
using Domain.Entities;
using Domain.IClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

// Add logging if you want detailed error logs

namespace ML_Model;

public class MlHttpClient : IMlHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MlHttpClient> _logger; // Inject ILogger for logging

    public MlHttpClient(HttpClient httpClient, IConfiguration configuration, ILogger<MlHttpClient> logger)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configuration["MLService:BaseUrl"]);
        _logger = logger; // Initialize logger
    }

    public async Task<PredictionLog?> PredictAsync(SensorReadingDto input)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/predict", input);
            response.EnsureSuccessStatusCode(); // Ensures success status code (throws exception if not)

            return await response.Content
                .ReadFromJsonAsync<PredictionLog>(); // Deserialize the response into PredictionLog
        }
        catch (HttpRequestException ex)
        {
            // Log the exception details for better debugging
            _logger.LogError(ex, "Error communicating with ML service.");
            throw new Exception("Error communicating with ML service.", ex);
        }
    }
}