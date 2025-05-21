using System.Net.Http.Json;
using Domain.DTOs;
using Domain.Entities;
using Domain.IClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ML_Model;

public class MlHttpClient : IMlHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MlHttpClient> _logger;

    public MlHttpClient(HttpClient httpClient, IConfiguration configuration, ILogger<MlHttpClient> logger)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configuration["MLService:BaseUrl"]);
        _logger = logger;
    }

    public async Task<PredictionResultDto> PredictNextWateringTimeAsync(IEnumerable<SensorReading> sensorData)
    {
        try
        {
            var dtoList = sensorData.Select(r => new SensorReadingDto
            {
                TimeStamp = r.TimeStamp,
                Value = r.Value
            }).ToList();

            var response = await _httpClient.PostAsJsonAsync("/predict-next-watering-time", dtoList);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PredictionResultDto>();

            if (result == null)
            {
                _logger.LogError("ML service returned null prediction.");
                throw new Exception("ML service returned null prediction.");
            }

            _logger.LogInformation("Predicted in {Time}, watering in {Hours} hours",
                result.PredictionTime, result.HoursUntilNextWatering);

            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error communicating with ML service.");
            throw new Exception("Failed to communicate with ML service.", ex);
        }
    }

}