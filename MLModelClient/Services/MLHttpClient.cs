using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Domain.DTOs;

namespace MLModelClient.Services;

public class MLHttpClient
{
    private readonly HttpClient _httpClient;

    public MLHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PredictionResultDto?> PredictAsync(SensorDataDto input)
    {
        var response = await _httpClient.PostAsJsonAsync("http://127.0.0.1:8000/predict", input);
        return await response.Content.ReadFromJsonAsync<PredictionResultDto>();
    }
}