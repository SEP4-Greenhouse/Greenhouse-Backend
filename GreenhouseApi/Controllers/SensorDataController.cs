using Domain.Entities;
using Domain.IServices;
using Microsoft.AspNetCore.Mvc;

namespace GreenhouseApi.Controllers;

[ApiController]
[Route("api/[actuator]")]
public class SensorDataController(ISensorService sensorService) : ControllerBase
{
    // Add a new sensor
    [HttpPost("sensor")]
    public async Task<IActionResult> AddSensor([FromBody] Sensor sensor)
    {
        await sensorService.AddAsync(sensor);
        return Ok("Sensor added successfully.");
    }

    // Add a new sensor reading
    [HttpPost("sensor/reading")]
    public async Task<IActionResult> AddSensorReading([FromBody] SensorReading reading)
    {
        await sensorService.AddSensorReadingAsync(reading);
        return Ok("Sensor reading added successfully.");
    }

    // Delete a sensor
    [HttpDelete("sensor/{sensorId}")]
    public async Task<IActionResult> DeleteSensor(int sensorId)
    {
        try
        {
            await sensorService.DeleteAsync(sensorId);
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
        var readings = await sensorService.GetLatestReadingFromAllSensorsAsync();
        return Ok(readings);
    }

    // Get all readings grouped by sensor
    [HttpGet("sensor/readings")]
    public async Task<IActionResult> GetReadingsBySensor()
    {
        var readings = await sensorService.GetReadingsBySensorAsync();
        return Ok(readings);
    }

    // Get the latest reading by each sensor
    [HttpGet("sensor/latest")]
    public async Task<IActionResult> GetLatestReadingBySensor()
    {
        var readings = await sensorService.GetLatestReadingBySensorAsync();
        return Ok(readings);
    }

    // Get readings by timestamp range
    [HttpGet("range")]
    public async Task<IActionResult> GetReadingsByTimestampRange([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var readings = await sensorService.GetReadingsByTimestampRangeAsync(start, end);
        return Ok(readings);
    }

    // Get paginated readings by sensor
    [HttpGet("sensor/{sensorId}/paginated")]
    public async Task<IActionResult> GetPaginatedReadings(int sensorId, [FromQuery] int pageNumber,
        [FromQuery] int pageSize)
    {
        var readings = await sensorService.GetReadingsPaginatedAsync(sensorId, pageNumber, pageSize);
        return Ok(readings);
    }

    // Get average reading for a sensor
    [HttpGet("sensor/{sensorId}/average")]
    public async Task<IActionResult> GetAverageReading(int sensorId, [FromQuery] DateTime start,
        [FromQuery] DateTime end)
    {
        var avg = await sensorService.GetAverageReadingForSensorAsync(sensorId, start, end);
        return Ok(avg);
    }
}