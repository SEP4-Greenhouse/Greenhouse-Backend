using Domain.Entities;
using Domain.IRepositories;
using Domain.IServices;

namespace GreenhouseService.Services;

public class ControllerService : IControllerService
{
    private readonly IControllerRepository _controllerRepository;

    public ControllerService(IControllerRepository controllerRepository)
    {
        _controllerRepository = controllerRepository;
    }

    public async Task<Controller> GetControllerByIdAsync(int id)
    {
        var controller = await _controllerRepository.GetByIdAsync(id);
        if (controller == null)
            throw new KeyNotFoundException("Controller not found.");
        return controller;
    }

    public async Task<IEnumerable<Controller>> GetControllersByGreenhouseIdAsync(int greenhouseId)
    {
        return await _controllerRepository.GetByGreenhouseIdAsync(greenhouseId);
    }

    public async Task<Controller> CreateControllerAsync(Controller controller)
    {
        if (controller == null)
            throw new ArgumentNullException(nameof(controller));

        if (await _controllerRepository.ExistsByIdAsync(controller.Id))
            throw new ArgumentException("A controller with the same ID already exists.");

        await _controllerRepository.AddAsync(controller);
        return controller;
    }

    public async Task<ControllerAction> TriggerControllerActionAsync(int controllerId, string actionType, double value)
    {
        var controller = await GetControllerByIdAsync(controllerId);
        var action = controller.InitiateAction(DateTime.UtcNow, actionType, value);
        await _controllerRepository.UpdateAsync(controller);
        return action;
    }

    public async Task UpdateControllerStatusAsync(int controllerId, string newStatus)
    {
        var controller = await GetControllerByIdAsync(controllerId);
        controller.UpdateStatus(newStatus);
        await _controllerRepository.UpdateAsync(controller);
    }

    public async Task<IEnumerable<ControllerAction>> GetControllerActionsAsync(int controllerId)
    {
        return await _controllerRepository.GetActionsByControllerIdAsync(controllerId);
    }

    public async Task DeleteControllerAsync(int controllerId)
    {
        await _controllerRepository.DeleteAsync(controllerId);
    }
}