using Action = Domain.Entities.Action;

namespace Domain.IRepositories;

public interface IActionRepository
{
    Task<Action?> GetByIdAsync(int id);
    Task<IEnumerable<Action>> GetAllAsync();
    Task AddAsync(Action action);
    Task UpdateAsync(Action action);
    Task DeleteAsync(int id);
}