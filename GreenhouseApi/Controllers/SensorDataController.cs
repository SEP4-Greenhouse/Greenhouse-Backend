using Domain.Entities;
using Domain.IServices;
using Microsoft.AspNetCore.Mvc;

namespace GreenhouseApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SensorDataController(ISensorDataService sensorDataService) : ControllerBase
{
    // Add a new sensor
    [HttpPost("sensor")]
    public async Task<IActionResult> AddSensor([FromBody] Sensor sensor)
    {
        await sensorDataService.AddSensorAsync(sensor);
        return Ok("Sensor added successfully.");
    }

    // Add a new sensor reading
    [HttpPost("sensor/reading")]
    public async Task<IActionResult> AddSensorReading([FromBody] SensorReading reading)
    {
        await sensorDataService.AddSensorReadingAsync(reading);
        return Ok("Sensor reading added successfully.");
    }

    // Delete a sensor
    [HttpDelete("sensor/{sensorId}")]
    public async Task<IActionResult> DeleteSensor(int sensorId)
    {
        try
        {
            await sensorDataService.DeleteSensorAsync(sensorId);
            return Ok("Sensor deleted successfully.");
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

    // Get latest reading from all sensors
    [HttpGet("latest/all")]
    public async Task<IActionResult> GetLatestReadingFromAllSensors()
    {
        var readings = await sensorDataService.GetLatestReadingFromAllSensorsAsync();
        return Ok(readings);
    }

    // Get all readings grouped by sensor
    [HttpGet("sensor/readings")]
    public async Task<IActionResult> GetReadingsBySensor()
    {
        var readings = await sensorDataService.GetReadingsBySensorAsync();
        return Ok(readings);
    }

    // Get the latest reading by each sensor
    [HttpGet("sensor/latest")]
    public async Task<IActionResult> GetLatestReadingBySensor()
    {
        var readings = await sensorDataService.GetLatestReadingBySensorAsync();
        return Ok(readings);
    }

    // Get readings by timestamp range
    [HttpGet("range")]
    public async Task<IActionResult> GetReadingsByTimestampRange([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var readings = await sensorDataService.GetReadingsByTimestampRangeAsync(start, end);
        return Ok(readings);
    }

    // Get paginated readings by sensor
    [HttpGet("sensor/{sensorId}/paginated")]
    public async Task<IActionResult> GetPaginatedReadings(int sensorId, [FromQuery] int pageNumber,
        [FromQuery] int pageSize)
    {
        var readings = await sensorDataService.GetReadingsPaginatedAsync(sensorId, pageNumber, pageSize);
        return Ok(readings);
    }

    // Get average reading for a sensor
    [HttpGet("sensor/{sensorId}/average")]
    public async Task<IActionResult> GetAverageReading(int sensorId, [FromQuery] DateTime start,
        [FromQuery] DateTime end)
    {
        var avg = await sensorDataService.GetAverageReadingForSensorAsync(sensorId, start, end);
        return Ok(avg);
    }

    // Get all alerts from sensor readings
    [HttpGet("alerts")]
    public async Task<IActionResult> GetAllSensorReadingAlerts()
    {
        var alerts = await sensorDataService.GetAllSensorsReadingAlertsAsync();
        return Ok(alerts);
    }
}