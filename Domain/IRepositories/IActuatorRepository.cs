using Domain.Entities;

namespace Domain.IRepositories;

public interface IActuatorRepository : IBaseRepository<Actuator>
{
    Task<IEnumerable<Actuator>> GetByGreenhouseIdAsync(int greenhouseId);
    Task<IEnumerable<ActuatorAction>> GetActionsByControllerIdAsync(int controllerId);
}