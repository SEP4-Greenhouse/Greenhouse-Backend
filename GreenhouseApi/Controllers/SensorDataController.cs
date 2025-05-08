using Domain.IServices;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SensorDataController : ControllerBase
{
    private readonly ISensorDataService _sensorDataService;

    public SensorDataController(ISensorDataService sensorDataService)
    {
        _sensorDataService = sensorDataService;
    }

    // Get latest reading from all sensors
    [HttpGet("latest/all")]
    public async Task<IActionResult> GetLatestReadingFromAllSensors()
    {
        var readings = await _sensorDataService.GetLatestReadingFromAllSensorsAsync();
        return Ok(readings);
    }

    // Get all readings grouped by sensor
    [HttpGet("sensor/readings")]
    public async Task<IActionResult> GetReadingsBySensor()
    {
        var readings = await _sensorDataService.GetReadingsBySensorAsync();
        return Ok(readings);
    }

    // Get the latest reading by each sensor
    [HttpGet("sensor/latest")]
    public async Task<IActionResult> GetLatestReadingBySensor()
    {
        var readings = await _sensorDataService.GetLatestReadingBySensorAsync();
        return Ok(readings);
    }

    // Get readings by timestamp range
    [HttpGet("range")]
    public async Task<IActionResult> GetReadingsByTimestampRange([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var readings = await _sensorDataService.GetReadingsByTimestampRangeAsync(start, end);
        return Ok(readings);
    }

    // Get paginated readings by sensor
    [HttpGet("sensor/{sensorId}/paginated")]
    public async Task<IActionResult> GetPaginatedReadings(int sensorId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var readings = await _sensorDataService.GetReadingsPaginatedAsync(sensorId, pageNumber, pageSize);
        return Ok(readings);
    }

    // Get average reading for a sensor
    [HttpGet("sensor/{sensorId}/average")]
    public async Task<IActionResult> GetAverageReading(int sensorId, [FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var avg = await _sensorDataService.GetAverageReadingForSensorAsync(sensorId, start, end);
        return Ok(avg);
    }

    // Get all alerts from sensor readings
    [HttpGet("alerts")]
    public async Task<IActionResult> GetAllSensorReadingAlerts()
    {
        var alerts = await _sensorDataService.GetAllSensorsReadingAlertsAsync();
        return Ok(alerts);
    }
}
