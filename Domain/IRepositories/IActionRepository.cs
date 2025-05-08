using Domain.Entities;

namespace Domain.IRepositories;

public interface IActionRepository
{
    Task<ControllerAction?> GetByIdAsync(int id);
    Task<IEnumerable<ControllerAction>> GetAllAsync();
    Task AddAsync(ControllerAction controllerAction);
    Task UpdateAsync(ControllerAction controllerAction);
    Task DeleteAsync(int id);
}