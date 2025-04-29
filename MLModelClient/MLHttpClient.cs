using System.Net.Http.Json;
using Domain.DTOs;
using Domain.IClients;

namespace ML_Model;

public class MLHttpClient : ImlHttpClient
{
    private readonly HttpClient _httpClient;

    public MLHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://127.0.0.1:8000"); // FastAPI URL
    }

    public async Task<PredictionResultDto?> PredictAsync(SensorDataDto input)
    {
        var response = await _httpClient.PostAsJsonAsync("/predict", input);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PredictionResultDto>();
    }
}