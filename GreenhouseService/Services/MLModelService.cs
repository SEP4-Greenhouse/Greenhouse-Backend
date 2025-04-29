using Domain.DTOs;
using Domain.Entities;
using Domain.IClients;
using Domain.IRepositories;
using Domain.IServices;

namespace GreenhouseService.Services;

public class MlModelService : IMlModelService
{
    private readonly ImlHttpClient _mlHttpClient;
    private readonly IPredictionLogRepository _predictLogRepository;

    public MlModelService(ImlHttpClient mlHttpClient, IPredictionLogRepository predictLogRepository)
    {
        _mlHttpClient = mlHttpClient;
        _predictLogRepository = predictLogRepository;
    }

    public async Task<PredictionLog> PredictAsync(SensorDataDto input)
    {
        var response = await _httpClient.PostAsJsonAsync("/predict", input);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PredictionResultDto>();
        
        if (result == null)
        {
            throw new Exception("Prediction result is null");
        }

        return result!;
    }
}