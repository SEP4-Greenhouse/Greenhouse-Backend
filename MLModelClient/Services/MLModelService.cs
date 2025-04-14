using System.Net.Http.Json;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;

namespace MLModelClient.Services;

public class MlModelService : IMlModelService
{
    private readonly HttpClient _httpClient;
    private readonly IPredictionLogRepository _predictLogRepository;

    public MlModelService(HttpClient httpClient, IPredictionLogRepository predictLogRepository)
    {
        _httpClient = httpClient;
        _predictLogRepository = predictLogRepository;
        _httpClient.BaseAddress = new Uri("http://127.0.0.1:8000"); // FastAPI URL
    }

    public async Task<PredictionResultDto> PredictAsync(SensorDataDto input)
    {
        var response = await _httpClient.PostAsJsonAsync("/predict", input);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PredictionResultDto>();

        if (result != null)
        {
            await _predictLogRepository.AddAsync(new PredictionLog
            {
                SensorType = input.SensorType,
                Value = input.Value,
                SensorTimestamp = input.Timestamp,
                Status = result.Status,
                Suggestion = result.Suggestion,
                PredictionTimestamp = result.Timestamp
            });
        }

        return result!;
    }
}