using Domain.DTOs;


namespace Domain.IClients;

public interface ImlHttpClient
{
    Task<PredictionResultDto?> PredictAsync(SensorDataDto input);
}