using Domain.Entities;
using Domain.IServices;
using Microsoft.AspNetCore.Mvc;

namespace GreenhouseApi.Controllers;

[ApiController]
[Route("api/actuator")]
public class ActuatorController(IActuatorService actuatorService) : ControllerBase
{
    //This Endpoint is supposed to be used to send Commands from the frontend to the IOT devices.
    //but for now it only adds it to the Database.
    //(For Example: Turn on the water pump, turn off the water pump, etc.)
    [HttpPost("{id}/action")]
    public async Task<IActionResult> TriggerActuatorAction(int id, [FromBody] ActuatorAction actuatorAction)
    {
        try
        {
            var triggeredAction = await actuatorService.TriggerActuatorActionAsync(
                id, actuatorAction.Type, actuatorAction.Value);
            return Ok(triggeredAction);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}