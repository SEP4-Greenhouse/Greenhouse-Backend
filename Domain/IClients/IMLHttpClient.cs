using Domain.DTOs;
using Domain.Entities;


namespace Domain.IClients;

public interface IMlHttpClient
{
    Task<PredictionResultDto> PredictNextWateringTimeAsync(IEnumerable<SensorReading> sensorData);
}