using Domain.Entities;

namespace Domain.IRepositories;

public interface IAlertRepository
{
    Task<Alert?> GetByIdAsync(int id);
    Task<IEnumerable<Alert>> GetAllAsync();
    Task<IEnumerable<Alert>> GetBySensorTypeAsync();
    Task AddAsync(Alert alert);
    Task UpdateAsync(Alert alert);
    Task DeleteAsync(int id);
}