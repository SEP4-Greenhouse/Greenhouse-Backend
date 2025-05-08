using Domain.Entities;

namespace Domain.IServices
{
    public interface IControllerService
    {
        Task<Controller> GetControllerByIdAsync(int id);
        Task<IEnumerable<Controller>> GetControllersByGreenhouseIdAsync(int greenhouseId);
        Task<Controller> CreateControllerAsync(Controller controller);
        Task<ControllerAction> TriggerControllerActionAsync(int controllerId, string actionType, double value);
        Task UpdateControllerStatusAsync(int controllerId, string newStatus);
        Task<IEnumerable<ControllerAction>> GetControllerActionsAsync(int controllerId);
        Task DeleteControllerAsync(int controllerId);
    }
}