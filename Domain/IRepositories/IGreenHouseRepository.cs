using Domain.Entities;

namespace Domain.IRepositories;

public interface IGreenhouseRepository
{
    Task<Greenhouse?> GetByIdAsync(int id);
    Task<IEnumerable<Greenhouse>> GetAllAsync();
    Task AddAsync(Greenhouse greenhouse);
    Task UpdateAsync(Greenhouse greenhouse);
    Task DeleteAsync(int id);
}