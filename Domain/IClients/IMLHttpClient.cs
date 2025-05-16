using Domain.DTOs;
using Domain.Entities;


namespace Domain.IClients;

public interface IMlHttpClient
{
    Task<PredictionLog?> PredictAsync(SensorReadingDto input);
}