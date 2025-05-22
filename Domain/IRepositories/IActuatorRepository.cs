using Domain.Entities;
using Domain.Entities.Actuators;

namespace Domain.IRepositories;

public interface IActuatorRepository : IBaseRepository<Actuator>
{
    Task<IEnumerable<Actuator>> GetByGreenhouseIdAsync(int greenhouseId);
    Task<IEnumerable<ActuatorAction>> GetActionsByActuatorIdAsync(int actuatorId);
}