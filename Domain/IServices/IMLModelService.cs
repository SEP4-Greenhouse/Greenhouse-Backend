using Domain.DTOs;
using Domain.Entities;

namespace Domain.IServices;

public interface IMlModelService
{ 
    Task<PredictionResultDto> PredictNextWateringTimeAsync(IEnumerable<SensorReading> data);
}