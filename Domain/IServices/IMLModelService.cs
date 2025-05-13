using Domain.DTOs;
using Domain.Entities;

namespace Domain.IServices;

public interface IMlModelService
{
    Task<PredictionLog?> PredictAsync(SensorDataDto data);
}