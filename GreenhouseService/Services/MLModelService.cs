using Domain.DTOs;
using Domain.Entities;
using Domain.IClients;
using Domain.IRepositories;
using Domain.IServices;
using Microsoft.Extensions.Logging;

namespace GreenhouseService.Services;

public class MlModelService(
    IMlHttpClient mlClient,
    ISensorReadingRepository sensorReadingRepo,
    ILogger<MlModelService> logger)
    : IMlModelService
{
    private readonly ILogger<MlModelService> _logger = logger;

    public async Task<PredictionResultDto> PredictNextWateringTimeAsync(IEnumerable<SensorReading> data)
    {
        var sensorData = await sensorReadingRepo.GetLatestFromAllSensorsAsync();

        var prediction = await mlClient.PredictNextWateringTimeAsync(sensorData);

        return prediction;
    }
}