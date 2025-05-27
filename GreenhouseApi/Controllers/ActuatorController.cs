using Domain.DTOs;
using Domain.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenhouseApi.Controllers;

[Authorize]
[ApiController]
[Route("api/actuator")]
public class ActuatorController(IActuatorService actuatorService) : ControllerBase
{
    [HttpPost("{id}/action")]

    //This Endpoint is supposed to be used to send Commands from the frontend to the IotClient devices.
    //but for now it only adds it to the Database.
    //(For Example: Turn on the water pump, turn off the water pump, etc.)
    public async Task<IActionResult> TriggerActuatorAction(int id, [FromBody] ActuatorActionDto actionDto)
    {
        await actuatorService.TriggerActuatorActionAsync(id, actionDto.Action, actionDto.Value);
        return Ok();
    }
}