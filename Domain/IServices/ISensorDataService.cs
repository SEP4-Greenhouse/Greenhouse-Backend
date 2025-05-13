namespace Domain.IServices
{
    using Entities;

    public interface ISensorDataService
    {
        Task<IEnumerable<SensorReading>> GetLatestReadingFromAllSensorsAsync();

        Task<IDictionary<int, IEnumerable<SensorReading>>> GetReadingsBySensorAsync();

        Task<IEnumerable<SensorReading>> GetReadingsByTimestampRangeAsync(DateTime start, DateTime end);

        Task<IEnumerable<SensorReading>> GetReadingsPaginatedAsync(int sensorId, int pageNumber, int pageSize);

        Task<double> GetAverageReadingForSensorAsync(int sensorId, DateTime start, DateTime end);

        Task<IDictionary<int, SensorReading>> GetLatestReadingBySensorAsync();

        Task AddSensorReadingAsync(SensorReading reading);

        Task TriggerAlertIfThresholdExceededAsync(SensorReading reading);

        Task<IEnumerable<Alert>> GetAllSensorsReadingAlertsAsync();

        Task AddSensorAsync(Sensor sensor);

        Task DeleteSensorAsync(int sensorId);
    }
}