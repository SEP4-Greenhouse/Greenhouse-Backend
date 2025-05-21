using Domain.DTOs;

namespace Domain.IClients;

public interface IMlHttpClient
{
    Task<PredictionResultDto> PredictNextWateringTimeAsync(MlModelDataDto preparedData);
}