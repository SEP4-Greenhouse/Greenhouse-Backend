using Domain.DTOs;
using Domain.Entities;

namespace Domain.IServices
{
    public interface ISensorService : IBaseService<Sensor>
    {
        Task AddSensorReadingAsync(SensorReading reading);
        
        Task<IEnumerable<SensorReading>> GetLatestReadingFromAllSensorsAsync();
        Task<IDictionary<int, IEnumerable<SensorReading>>> GetReadingsBySensorAsync();
        Task<IEnumerable<SensorReading>> GetReadingsByTimestampRangeAsync(DateTime start, DateTime end);
        Task<IEnumerable<SensorReading>> GetReadingsPaginatedAsync(int sensorId, int pageNumber, int pageSize);
        Task<double> GetAverageReadingForSensorAsync(int sensorId, DateTime start, DateTime end);
        Task<IDictionary<int, SensorReading>> GetLatestReadingBySensorAsync();
        Task<Threshold?> GetThresholdBySensorIdAsync(int sensorId);
        Task<Threshold> AddThresholdToSensorAsync(int sensorId, ThresholdDto thresholdDto);
        Task<Threshold> UpdateThresholdAsync(Threshold threshold);
        Task<bool> DeleteThresholdAsync(int sensorId);
    }
}