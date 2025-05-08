using Domain.Entities;
using Action = Domain.Entities.Action;

namespace Domain.IServices
{
    public interface IControllerService
    {
        Task<Controller> GetControllerByIdAsync(int id);
        Task<IEnumerable<Controller>> GetControllersByGreenhouseIdAsync(int greenhouseId);
        Task<Controller> CreateControllerAsync(string type, string status, Greenhouse greenhouse);
        Task<Action> TriggerControllerActionAsync(int controllerId, string actionType, double value);
        Task UpdateControllerStatusAsync(int controllerId, string newStatus);
        Task<IEnumerable<Action>> GetControllerActionsAsync(int controllerId);
    }
}