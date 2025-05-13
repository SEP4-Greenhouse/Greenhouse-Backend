using Domain.Entities;

namespace Domain.IRepositories;

public interface ISensorReadingRepository : IBaseRepository<SensorReading>
{
    Task<IEnumerable<SensorReading>> GetLatestFromAllSensorsAsync();
    Task<IDictionary<int, IEnumerable<SensorReading>>> GetAllGroupedBySensorAsync();
    Task<IEnumerable<SensorReading>> GetByTimeRangeAsync(DateTime start, DateTime end);
    Task<IEnumerable<SensorReading>> GetPaginatedAsync(int sensorId, int pageNumber, int pageSize);
    Task<double> GetAverageAsync(int sensorId, DateTime start, DateTime end);
    Task<IDictionary<int, SensorReading>> GetLatestBySensorAsync();
}