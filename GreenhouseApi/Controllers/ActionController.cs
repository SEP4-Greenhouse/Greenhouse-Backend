using Domain.Entities;
using Domain.IServices;
using Microsoft.AspNetCore.Mvc;

namespace GreenhouseApi.Controllers;

[ApiController]
[Route("api/actuator")]
public class ActuatorController(IActuatorService actuatorService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateActuator([FromBody] Actuator actuator)
    {
        try
        {
            var createdActuator = await actuatorService.AddAsync(actuator);
            return Ok(createdActuator);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetActuatorById(int id)
    {
        try
        {
            var actuator = await actuatorService.GetByIdAsync(id);
            return Ok(actuator);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActuator(int id)
    {
        try
        {
            await actuatorService.DeleteAsync(id);
            return Ok("Actuator deleted successfully.");
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

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateActuatorStatus(int id, [FromBody] string newStatus)
    {
        if (string.IsNullOrWhiteSpace(newStatus))
            return BadRequest("Status cannot be empty.");

        try
        {
            await actuatorService.UpdateActuatorStatusAsync(id, newStatus);
            return Ok("Actuator status updated successfully.");
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