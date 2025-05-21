using System.Net.Http.Json;
using Domain.DTOs;
using Domain.IClients;
using Microsoft.Extensions.Configuration;

namespace ML_Model;

public class MlHttpClient : IMlHttpClient
{
    private readonly HttpClient _httpClient;

    public MlHttpClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configuration["MLService:BaseUrl"]);
    }

    public async Task<PredictionResultDto> PredictNextWateringTimeAsync(MlModelDataDto preparedData)
    {
        var Json = System.Text.Json.JsonSerializer.Serialize(preparedData);
        Console.WriteLine(Json);
        var response = await _httpClient.PostAsJsonAsync("api/ml/predict", preparedData);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PredictionResultDto>();
        if (result == null)
            throw new Exception("ML service returned null prediction.");

        return result;
    }
}