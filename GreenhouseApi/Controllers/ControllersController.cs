using Domain.Entities;
using Domain.IServices;
using Microsoft.AspNetCore.Mvc;
using Controller = Domain.Entities.Controller;

namespace GreenhouseApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ControllersController : ControllerBase
{
    private readonly IControllerService _controllerService;

    public ControllersController(IControllerService controllerService)
    {
        _controllerService = controllerService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateController([FromBody] Controller controller)
    {
        try
        {
            var createdController = await _controllerService.CreateControllerAsync(controller);
            return Ok(createdController);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetControllerById(int id)
    {
        try
        {
            var controller = await _controllerService.GetControllerByIdAsync(id);
            return Ok(controller);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteController(int id)
    {
        try
        {
            await _controllerService.DeleteControllerAsync(id);
            return Ok("Controller deleted successfully.");
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
    public async Task<IActionResult> UpdateControllerStatus(int id, [FromBody] string newStatus)
    {
        if (string.IsNullOrWhiteSpace(newStatus))
            return BadRequest("Status cannot be empty.");

        try
        {
            await _controllerService.UpdateControllerStatusAsync(id, newStatus);
            return Ok("Controller status updated successfully.");
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

    [HttpPost("{id}/controllerAction")]
    public async Task<IActionResult> TriggerControllerAction(int id, [FromBody] ControllerAction controllerAction)
    {
        try
        {
            var triggeredAction = await _controllerService.TriggerControllerActionAsync(
                id, controllerAction.Type, controllerAction.Value);
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