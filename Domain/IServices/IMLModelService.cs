using Domain.DTOs;
using Domain.Entities;

namespace Domain.IServices;

public interface IMlModelService
{
    Task<PredictionResultDto> PredictNextWateringTimeAsync(MlModelDataDto preparedData, int plantId);
    Task PrepareDataForPredictionAsync(MlModelDataDto data, int plantId);
    Task<IEnumerable<PredictionLog>> GetAllPredictionLogsAsync();
}