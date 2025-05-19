using Domain.DTOs;
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
    public async Task<IActionResult> TriggerActuatorAction(int id, [FromBody] ActuatorActionDto actionDto)
    {
        try
        {
            await actuatorService.TriggerActuatorActionAsync(
                id, actionDto.Action, actionDto.Value);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}