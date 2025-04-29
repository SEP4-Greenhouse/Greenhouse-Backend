// Updated MlModelService class
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
        var result = await _mlHttpClient.PredictAsync(input);

        if (result != null)
        {
            await _predictLogRepository.AddAsync(new PredictionLog
            {
                SensorType = input.Current.SensorType,
                Value = input.Current.Value,
                SensorTimestamp = input.Current.Timestamp,
                Status = result.Status,
                Suggestion = result.Suggestion,
                PredictionTimestamp = result.PredictionTimestamp
            });
        }

        return result!;
    }
}