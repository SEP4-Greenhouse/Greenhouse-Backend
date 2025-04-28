using Domain.Entities;

namespace Domain.IRepositories;

public interface IPlantRepository
{
    Task<Plant?> GetByIdAsync(int id);
    Task<IEnumerable<Plant>> GetAllAsync();
    Task AddAsync(Plant plant);
    Task UpdateAsync(Plant plant);
    Task DeleteAsync(int id);
}