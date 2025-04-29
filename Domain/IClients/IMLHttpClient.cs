
using Domain.DTOs;
using Domain.Entities;


namespace Domain.IClients;

public interface ImlHttpClient
{

    Task<PredictionLog?> PredictAsync(SensorDataDto input);
}