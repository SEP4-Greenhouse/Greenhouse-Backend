using System.Net.Http.Json;
using Domain.DTOs;
using Domain.IClients;
using Microsoft.Extensions.Configuration;

namespace ML_Model;

public class MlHttpClient(HttpClient httpClient) : IMlHttpClient
{
    public async Task<PredictionResultDto> PredictNextWateringTimeAsync(MlModelDataDto preparedData)
    {
        var response = await httpClient.PostAsJsonAsync("api/ml/predict", preparedData);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PredictionResultDto>();
        if (result == null)
            throw new Exception("ML service returned null prediction.");

        return result;
    }
}
