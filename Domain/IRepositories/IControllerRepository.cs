using Domain.Entities;

namespace Domain.IRepositories;

public interface IControllerRepository : IBaseRepository<Controller>
{
    Task<IEnumerable<Controller>> GetByGreenhouseIdAsync(int greenhouseId);
    Task<IEnumerable<ControllerAction>> GetActionsByControllerIdAsync(int controllerId);
}