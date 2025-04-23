using Domain.DTOs;

namespace MLModelClient.Services;


public interface IMlModelService
{
    Task<PredictionResultDto> PredictAsync(SensorDataDto data);
}