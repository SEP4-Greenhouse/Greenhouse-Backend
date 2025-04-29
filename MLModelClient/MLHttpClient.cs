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
        _httpClient.BaseAddress = new Uri("http://host.docker.internal:8000");// Set the base URL
    }

    public async Task<PredictionLog?> PredictAsync(SensorDataDto input)
    {
        var response = await _httpClient.PostAsJsonAsync("/predict", input); // Use relative endpoint
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PredictionLog>();
    }
}