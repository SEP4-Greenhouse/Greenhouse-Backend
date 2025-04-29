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

    public async Task<PredictionLog?> PredictAsync(SensorDataDto input)
    {
        if (input == null || input.current == null || input.history == null || !input.history.Any())
        {
            throw new ArgumentException("Invalid sensor data input.");
        }

        var result = await _mlHttpClient.PredictAsync(input);

        if (result == null)
        {
            return null;
        }

        var predictionLog = new PredictionLog
        {
            Timestamp = result.Timestamp,  
            Status = result.Status, 
            Suggestion = result.Suggestion,  
            TrendAnalysis = result.TrendAnalysis 
        };

        await _predictLogRepository.AddAsync(predictionLog);

        return result;
    }
}