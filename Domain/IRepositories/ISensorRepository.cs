using Domain.Entities;

namespace Domain.IRepositories;

public interface ISensorRepository
{
    Task<Sensor?> GetByIdAsync(int id);
    Task<IEnumerable<Sensor>> GetAllAsync();
    Task AddAsync(Sensor sensor);
    Task UpdateAsync(Sensor sensor);
    Task DeleteAsync(int id);
}