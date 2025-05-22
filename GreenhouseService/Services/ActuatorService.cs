using Domain.Entities;
using Domain.Entities.Actuators;
using Domain.IRepositories;
using Domain.IServices;

namespace GreenhouseService.Services;

public class ActuatorService(
    IActuatorRepository actuatorRepository,
    IAlertService alertService)
    : BaseService<Actuator>(actuatorRepository), IActuatorService
{
    public async Task<ActuatorAction> TriggerActuatorActionAsync(int actuatorId, string actionType, double value)
    {
        var actuator = await GetByIdAsync(actuatorId);
        var action = actuator?.InitiateAction(DateTime.UtcNow, actionType, value);
        await UpdateAsync(actuator ?? throw new InvalidOperationException());

        if (action != null)
        {
            await alertService.CreateActuatorAlertAsync(
                action,
                $"Actuator action triggered: {actionType} with value {value} on actuator {actuatorId}"
            );
        }

        return action ?? throw new InvalidOperationException();
    }
}