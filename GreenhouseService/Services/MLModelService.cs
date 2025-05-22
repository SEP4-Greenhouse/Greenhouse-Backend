using Domain.DTOs;
using Domain.Entities;
using Domain.IClients;
using Domain.IRepositories;
using Domain.IServices;

namespace GreenhouseService.Services;

public class MlModelService(
    IMlHttpClient mlClient,
    ISensorReadingRepository sensorReadingRepo,
    IPlantRepository plantRepository,
    IPredictionLogRepository predictionLogRepository)
    : IMlModelService
{
    public async Task<PredictionResultDto> PredictNextWateringTimeAsync(MlModelDataDto preparedData, int plantId)
    {
        var prediction = await mlClient.PredictNextWateringTimeAsync(preparedData);

        var log = new PredictionLog
        {
            PlantId = plantId,
            PredictionTime = prediction.PredictionTime,
            HoursUntilNextWatering = prediction.HoursUntilNextWatering
        };
        await AddPredictionLogAsync(log);

        return prediction;
    }

    public async Task PrepareDataForPredictionAsync(MlModelDataDto data, int plantId)
    {
        var plant = await plantRepository.GetByIdAsync(plantId);
        if (plant == null) throw new Exception("Plant not found");

        var greenhouse = plant.Greenhouse;
        var sensors = greenhouse.Sensors;
        var sensorIds = sensors.Select(s => s.Id).ToList();

        var allLatestReadings = await sensorReadingRepo.GetLatestFromAllSensorsAsync();
        var sensorReadings = allLatestReadings
            .Where(r => sensorIds.Contains(r.SensorId))
            .ToList();

        var actuators = greenhouse.Actuators;
        var waterPump = actuators.FirstOrDefault(a => a is WaterPumpActuator);

        ActuatorAction? lastWateringAction = null;
        if (waterPump?.Actions.Any() == true)
        {
            var wateringActions = new[] { "TurnOn", "SetFlowRate", "Turned On", "Watering On", "water on" };
            lastWateringAction = waterPump.Actions
                .Where(a => wateringActions.Contains(a.Action, StringComparer.OrdinalIgnoreCase))
                .OrderByDescending(a => a.Timestamp)
                .FirstOrDefault();
        }

        double timeSinceLastWateringHours = lastWateringAction != null
            ? (DateTime.UtcNow - lastWateringAction.Timestamp).TotalHours
            : -1;

        var mlSensorReadings = sensorReadings.Select(r =>
        {
            var sensor = sensors.FirstOrDefault(s => s.Id == r.SensorId);
            return sensor == null
                ? null
                : new MlSensorReadingDto
                {
                    SensorName = sensor.Type,
                    Unit = sensor.Unit,
                    Value = r.Value
                };
        }).Where(dto => dto != null).ToList();

        data.Timestamp = DateTime.UtcNow;
        data.PlantGrowthStage = plant.GrowthStage;
        data.TimeSinceLastWateringInHours = timeSinceLastWateringHours;
        data.MlSensorReadings = mlSensorReadings;
    }

    public async Task<IEnumerable<PredictionLog>> GetAllPredictionLogsAsync()
    {
        return await predictionLogRepository.GetAllAsync();
    }

    public async Task<PredictionLog> AddPredictionLogAsync(PredictionLog log)
    {
        if (log == null)
            throw new ArgumentNullException(nameof(log), "Prediction log cannot be null.");
        return await predictionLogRepository.AddAsync(log);
    }
}