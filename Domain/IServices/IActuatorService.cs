using Domain.Entities;

namespace Domain.IServices
{
    public interface IActuatorService : IBaseService<Actuator>
    {
        Task<IEnumerable<Actuator>> GetActuatorsByGreenhouseIdAsync(int greenhouseId);
        Task<ActuatorAction> TriggerActuatorActionAsync(int actuatorId, string actionType, double value);
        Task UpdateActuatorStatusAsync(int actuatorId, string newStatus);

        Task<IEnumerable<ActuatorAction>> GetActuatorActionsAsync(int actuatorId);

        Task<ActuatorAction> GetActionByIdAsync(int actionId);
        Task<IEnumerable<ActuatorAction>> GetAllActionsAsync();
        Task<ActuatorAction> AddActionAsync(ActuatorAction action);
        Task UpdateActionAsync(ActuatorAction action);
        Task DeleteActionAsync(int actionId);
    }
}