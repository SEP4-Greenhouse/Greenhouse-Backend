using System.Net.Http.Json;
using Domain.DTOs;
using Domain.Entities;
using Domain.IClients;

namespace ML_Model;

public class MLHttpClient : ImlHttpClient
{
    private readonly HttpClient _httpClient;

    public MLHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
<<<<<<< Updated upstream
=======
        _httpClient.BaseAddress = new Uri("http://host.docker.internal:8000"); // FastAPI URL for Docker
>>>>>>> Stashed changes
    }

    public async Task<PredictionLog?> PredictAsync(SensorDataDto input)
    {
<<<<<<< Updated upstream
        var response = await _httpClient.PostAsJsonAsync("http://127.0.0.1:8000/predict", input);
        return await response.Content.ReadFromJsonAsync<PredictionResultDto>();
=======
        var response = await _httpClient.PostAsJsonAsync("/predict", input);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PredictionLog>();
>>>>>>> Stashed changes
    }
}