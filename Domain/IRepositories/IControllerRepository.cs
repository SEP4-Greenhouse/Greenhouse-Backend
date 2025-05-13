using Domain.Entities;

namespace Domain.IRepositories;

public interface IControllerRepository
{
    Task<Controller?> GetByIdAsync(int id);
    Task<IEnumerable<Controller>> GetAllAsync();
    Task<IEnumerable<Controller>> GetByGreenhouseIdAsync(int greenhouseId);
    Task AddAsync(Controller controller);
    Task UpdateAsync(Controller controller);
    Task DeleteAsync(int id);
    Task<bool> ExistsByIdAsync(int id);
    Task<IEnumerable<ControllerAction>> GetActionsByControllerIdAsync(int controllerId);
}