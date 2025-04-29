using System.Net.Http.Json;
using Domain.DTOs;
using Domain.Entities;
using Domain.IRepositories;
using Domain.IServices;

namespace GreenhouseService.Services;

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

    public async Task<PredictionLog> PredictAsync(SensorDataDto input)
    {
<<<<<<< Updated upstream
        var response = await _httpClient.PostAsJsonAsync("/predict", input);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PredictionResultDto>();

        if (result != null)
=======
        var result = await _mlHttpClient.PredictAsync(input);
        if (result == null)
>>>>>>> Stashed changes
        {
            throw new Exception("Prediction result is null");
        }

        return result!;
    }
}