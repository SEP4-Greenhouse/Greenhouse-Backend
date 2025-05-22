using Domain.DTOs;
using Domain.Entities;
using Domain.IRepositories;
using Domain.IServices;

namespace GreenhouseService.Services;

public class SensorService(
    ISensorRepository sensorRepository,
    ISensorReadingRepository sensorReadingRepository,
    IThresholdRepository thresholdRepository,
    IAlertService alertService)
    : BaseService<Sensor>(sensorRepository), ISensorService
{

    public async Task AddSensorReadingAsync(SensorReading reading)
    {
        await sensorReadingRepository.AddAsync(reading);

        await CheckReadingThresholdsAsync(reading);
    }

    public async Task<IEnumerable<SensorReading>> GetLatestReadingFromAllSensorsAsync()
    {
        return await sensorReadingRepository.GetLatestFromAllSensorsAsync();
    }

    public async Task<IDictionary<int, IEnumerable<SensorReading>>> GetReadingsBySensorAsync()
    {
        return await sensorReadingRepository.GetAllGroupedBySensorAsync();
    }

    public async Task<IEnumerable<SensorReading>> GetReadingsByTimestampRangeAsync(DateTime start, DateTime end)
    {
        return await sensorReadingRepository.GetByTimeRangeAsync(start, end);
    }

    public async Task<IEnumerable<SensorReading>> GetReadingsPaginatedAsync(int sensorId, int pageNumber,
        int pageSize)
    {
        return await sensorReadingRepository.GetPaginatedAsync(sensorId, pageNumber, pageSize);
    }

    public async Task<double> GetAverageReadingForSensorAsync(int sensorId, DateTime start, DateTime end)
    {
        return await sensorReadingRepository.GetAverageAsync(sensorId, start, end);
    }

    public async Task<IDictionary<int, SensorReading>> GetLatestReadingBySensorAsync()
    {
        return await sensorReadingRepository.GetLatestBySensorAsync();
    }

    private async Task CheckReadingThresholdsAsync(SensorReading reading)
    {
        var threshold = await thresholdRepository.GetThresholdBySensorIdAsync(reading.SensorId);

        if (reading.Value < threshold.MinValue)
        {
            await alertService.CreateSensorAlertAsync(reading,
                $"Value below minimum threshold: {reading.Value} {reading.Unit} (Min: {threshold.MinValue})");
        }
        else if (reading.Value > threshold.MaxValue)
        {
            await alertService.CreateSensorAlertAsync(reading,
                $"Value above maximum threshold: {reading.Value} {reading.Unit} (Max: {threshold.MaxValue})");

        }
    }
    public async Task<Threshold> AddThresholdToSensorAsync(int sensorId, ThresholdDto thresholdDto)
    {
        var sensor = await sensorRepository.GetByIdAsync(sensorId);
        if (sensor == null)
            throw new ArgumentException($"Sensor with ID {sensorId} not found.");

        var threshold = new Threshold
        {
            SensorId = sensorId,
            MinValue = thresholdDto.MinValue,
            MaxValue = thresholdDto.MaxValue
        };

        return await thresholdRepository.AddThresholdAsync(threshold);
    }

    public async Task<Threshold> UpdateThresholdAsync(Threshold threshold)
    {
        var existingThreshold = await thresholdRepository.GetThresholdBySensorIdAsync(threshold.SensorId);
        if (existingThreshold == null)
            throw new ArgumentException($"Threshold for sensor {threshold.SensorId} not found.");

        existingThreshold.MinValue = threshold.MinValue;
        existingThreshold.MaxValue = threshold.MaxValue;

        return await thresholdRepository.UpdateThresholdAsync(existingThreshold);
    }

    public async Task<bool> DeleteThresholdAsync(int sensorId)
    {
        var threshold = await thresholdRepository.GetThresholdBySensorIdAsync(sensorId);
        if (threshold == null)
            throw new ArgumentException($"Threshold for sensor {sensorId} not found.");

        return await thresholdRepository.DeleteThresholdAsync(threshold.Id);
    }

    public async Task<Threshold?> GetThresholdBySensorIdAsync(int sensorId)
    {
        return await thresholdRepository.GetThresholdBySensorIdAsync(sensorId);
    }
    
}