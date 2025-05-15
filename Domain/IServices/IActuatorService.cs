using Domain.Entities;

namespace Domain.IServices
{
    public interface IActuatorService : IBaseService<Actuator>
    {
        Task<ActuatorAction> TriggerActuatorActionAsync(int actuatorId, string actionType, double value);
    }
}