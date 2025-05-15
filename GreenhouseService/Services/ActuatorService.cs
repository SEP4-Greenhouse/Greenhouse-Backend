using Domain.Entities;
using Domain.IRepositories;
using Domain.IServices;

namespace GreenhouseService.Services;

public class ActuatorService(
    IActuatorRepository actuatorRepository,
    IActuatorActionRepository actionRepository)
    : BaseService<Actuator>(actuatorRepository), IActuatorService
{
    public async Task<ActuatorAction> TriggerActuatorActionAsync(int actuatorId, string actionType, double value)
    {
        var actuator = await GetByIdAsync(actuatorId);
        var action = actuator.InitiateAction(DateTime.UtcNow, actionType, value);
        await UpdateAsync(actuator);
        return action;
    }
}