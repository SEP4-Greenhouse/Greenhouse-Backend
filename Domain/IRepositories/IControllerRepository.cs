using Domain.Entities;

namespace Domain.IRepositories;

public interface IControllerRepository
{
    Task<Controller?> GetByIdAsync(int id);
    Task<IEnumerable<Controller>> GetAllAsync();
    Task AddAsync(Controller controller);
    Task UpdateAsync(Controller controller);
    Task DeleteAsync(int id);
}