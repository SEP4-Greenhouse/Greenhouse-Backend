using Domain.DTOs;
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
        var dto = alerts.Select(a => new AlertDto(a.Message)
        {
            Type = a.Type.ToString(),
            Timestamp = a.Timestamp
        });
        return Ok(dto);
    }

    [HttpGet("range")]
    public async Task<IActionResult> GetAlertsByDateRange([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var alerts = await alertService.GetAlertsByDateRangeAsync(start, end);
        var dto = alerts.Select(a => new AlertDto(a.Message)
        {
            Type = a.Type.ToString(),
            Timestamp = a.Timestamp
        });
        return Ok(dto);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllAlerts()
    {
        var alerts = await alertService.GetAllAlertsAsync();
        var dto = alerts.Select(a => new AlertDto(a.Message)
        {
            Type = a.Type.ToString(),
            Timestamp = a.Timestamp
        });
        return Ok(dto);
    }
}