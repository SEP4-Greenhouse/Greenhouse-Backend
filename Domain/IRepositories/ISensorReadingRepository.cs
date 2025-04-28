using Domain.Entities;

namespace Domain.IRepositories;

public interface ISensorReadingRepository
{
    Task<SensorReading?> GetByIdAsync(int id);
    Task<IEnumerable<SensorReading>> GetAllAsync();
    Task AddAsync(SensorReading sensorReading);
    Task UpdateAsync(SensorReading sensorReading);
    Task DeleteAsync(int id);
}