using Domain.DTOs;

namespace Domain.IServices;


public interface IMlModelService
{
    Task<PredictionResultDto> PredictAsync(SensorDataDto data);
}