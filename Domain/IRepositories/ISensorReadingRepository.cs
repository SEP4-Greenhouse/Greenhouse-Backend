using Domain.Entities;

namespace Domain.IRepositories;

public interface ISensorReadingRepository
{
    Task<SensorReading?> GetByIdAsync(int id);
    Task<IEnumerable<SensorReading>> GetAllAsync();
    Task<IEnumerable<SensorReading>> GetLatestFromAllSensorsAsync();
    Task<IDictionary<int, IEnumerable<SensorReading>>> GetAllGroupedBySensorAsync();
    Task<IEnumerable<SensorReading>> GetByTimeRangeAsync(DateTime start, DateTime end);
    Task<IEnumerable<SensorReading>> GetPaginatedAsync(int sensorId, int pageNumber, int pageSize);
    Task<double> GetAverageAsync(int sensorId, DateTime start, DateTime end);
    Task<IDictionary<int, SensorReading>> GetLatestBySensorAsync();
    Task AddAsync(SensorReading sensorReading);
    Task UpdateAsync(SensorReading sensorReading);
    Task DeleteAsync(int id);
}