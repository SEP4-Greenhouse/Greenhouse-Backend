namespace Domain.IServices;

using Domain.Entities;

public interface ISensorDataService
{
    Task<SensorReading?> GetReadingByIdAsync(int id);
    Task<IEnumerable<SensorReading>> GetReadingsBySensorIdAsync(int sensorId);
    Task<IEnumerable<SensorReading>> GetLatestReadingsAsync(int count);
    Task AddSensorReadingAsync(SensorReading reading);
    Task<IEnumerable<SensorReading>> GetReadingsInRangeAsync(int sensorId, DateTime start, DateTime end);
    Task TriggerAlertIfNecessaryAsync(SensorReading reading);
    Task<IEnumerable<SensorReading>> GetLatestReadingsForAllSensorsAsync();
    Task<IEnumerable<SensorReading>> GetReadingsBySensorTypeAsync(string sensorType);
}