using Domain.Entities;

namespace Domain.IServices
{
    public interface ISensorService : IBaseService<Sensor>
    {
        // SensorReading CRUD operations
        Task AddSensorReadingAsync(SensorReading reading);
        
        // SensorReading query methods
        Task<IEnumerable<SensorReading>> GetLatestReadingFromAllSensorsAsync();
        Task<IDictionary<int, IEnumerable<SensorReading>>> GetReadingsBySensorAsync();
        Task<IEnumerable<SensorReading>> GetReadingsByTimestampRangeAsync(DateTime start, DateTime end);
        Task<IEnumerable<SensorReading>> GetReadingsPaginatedAsync(int sensorId, int pageNumber, int pageSize);
        Task<double> GetAverageReadingForSensorAsync(int sensorId, DateTime start, DateTime end);
        Task<IDictionary<int, SensorReading>> GetLatestReadingBySensorAsync();
    }
}