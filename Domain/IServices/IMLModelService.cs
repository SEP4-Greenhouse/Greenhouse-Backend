using Domain.DTOs;

namespace Domain.IServices;

public interface IMlModelService
{
    Task<PredictionResultDto> PredictNextWateringTimeAsync(MlModelDataDto preparedData);
    Task PrepareDataForPredictionAsync(MlModelDataDto data, int plantId);
}