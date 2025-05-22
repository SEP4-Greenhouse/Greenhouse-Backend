using Domain.Entities;
using Domain.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenhouseApi.Controllers;

[Authorize]
[ApiController]
[Route("api/alerts")]
public class AlertsController(IAlertService alertService) : ControllerBase
{
    [HttpGet("type/{type}")]
    public async Task<IActionResult> GetAlertsByType(Alert.AlertType type)
    {
        var alerts = await alertService.GetAlertsByTypeAsync(type);
        return Ok(alerts);
    }

    [HttpGet("range")]
    public async Task<IActionResult> GetAlertsByDateRange([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var alerts = await alertService.GetAlertsByDateRangeAsync(start, end);
        return Ok(alerts);
    }
    [HttpGet("all")]
    public async Task<IActionResult> GetAllAlerts()
    {
        var alerts = await alertService.GetAllAlertsAsync();
        return Ok(alerts);
    }
    
}