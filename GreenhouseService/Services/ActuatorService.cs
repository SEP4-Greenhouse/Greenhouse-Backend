using Domain.Entities;
using Domain.IRepositories;
using Domain.IServices;

namespace GreenhouseService.Services;

public class ActuatorService(
    IActuatorRepository actuatorRepository,
    IActuatorActionRepository actionRepository)
    : BaseService<Actuator>(actuatorRepository), IActuatorService
{
    public async Task<IEnumerable<Actuator>> GetActuatorsByGreenhouseIdAsync(int greenhouseId)
    {
        return await actuatorRepository.GetByGreenhouseIdAsync(greenhouseId);
    }

    public async Task<ActuatorAction> TriggerActuatorActionAsync(int actuatorId, string actionType, double value)
    {
        var actuator = await GetByIdAsync(actuatorId);
        var action = actuator.InitiateAction(DateTime.UtcNow, actionType, value);
        await UpdateAsync(actuator);
        return action;
    }

    public async Task UpdateActuatorStatusAsync(int actuatorId, string newStatus)
    {
        var actuator = await GetByIdAsync(actuatorId);
        actuator.UpdateStatus(newStatus);
        await UpdateAsync(actuator);
    }

    public async Task<IEnumerable<ActuatorAction>> GetActuatorActionsAsync(int actuatorId)
    {
        return await actuatorRepository.GetActionsByControllerIdAsync(actuatorId);
    }

    public async Task<ActuatorAction> GetActionByIdAsync(int actionId)
    {
        return await actionRepository.GetByIdAsync(actionId);
    }

    public async Task<IEnumerable<ActuatorAction>> GetAllActionsAsync()
    {
        return await actionRepository.GetAllAsync();
    }

    public async Task<ActuatorAction> AddActionAsync(ActuatorAction action)
    {
        return await actionRepository.AddAsync(action);
    }

    public async Task UpdateActionAsync(ActuatorAction action)
    {
        await actionRepository.UpdateAsync(action);
    }

    public async Task DeleteActionAsync(int actionId)
    {
        await actionRepository.DeleteAsync(actionId);
    }
}