using Domain.DTOs;
using Domain.Entities;
using Domain.IClients;
using Domain.IRepositories;
using Domain.IServices;

namespace GreenhouseService.Services;

public class MlModelService(
    IMlHttpClient mlClient,
    ISensorReadingRepository sensorReadingRepo,
    IPlantRepository plantRepository)
    : IMlModelService
{
    public async Task<PredictionResultDto> PredictNextWateringTimeAsync(MlModelDataDto preparedData)
    {
        var prediction = await mlClient.PredictNextWateringTimeAsync(preparedData);
        return prediction;
    }

    public async Task PrepareDataForPredictionAsync(MlModelDataDto data, int plantId)
    {
        var plant = await plantRepository.GetByIdAsync(plantId);
        if (plant == null) throw new Exception("Plant not found");

        var greenhouse = plant.Greenhouse;
        var sensors = greenhouse.Sensors ?? new List<Sensor>();
        var sensorIds = sensors.Select(s => s.Id).ToList();

        var allLatestReadings = await sensorReadingRepo.GetLatestFromAllSensorsAsync();
        var sensorReadings = allLatestReadings
            .Where(r => sensorIds.Contains(r.SensorId))
            .ToList();

        var actuators = greenhouse.Actuators ?? new List<Actuator>();
        // Find the water pump actuator (by type)
        var waterPump = actuators.FirstOrDefault(a => a is WaterPumpActuator);

        ActuatorAction? lastWateringAction = null;
        if (waterPump?.Actions?.Any() == true)
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
        Console.WriteLine($"Sensors in greenhouse: {string.Join(", ", sensors.Select(s => s.Id))}");
        Console.WriteLine(
            $"All latest readings: {string.Join(", ", allLatestReadings.Select(r => $"{r.SensorId}:{r.Value}"))}");
        Console.WriteLine(
            $"Filtered readings: {string.Join(", ", sensorReadings.Select(r => $"{r.SensorId}:{r.Value}"))}");
        data.MlSensorReadings = mlSensorReadings;
    }
}

public static class ActuatorActions
{
    public const string TurnOn = "TurnOn";
    public const string SetFlowRate = "SetFlowRate";
}