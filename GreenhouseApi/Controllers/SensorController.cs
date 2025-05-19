using Domain.DTOs;
using Domain.Entities;
using Domain.IServices;
using Microsoft.AspNetCore.Mvc;

namespace GreenhouseApi.Controllers;

[ApiController]
[Route("api/sensor")]
public class SensorController(ISensorService sensorService) : ControllerBase
{
    
    // This Endpoint is for IOT Team to add a new sensor reading into the database.
    [HttpPost("sensor/reading(IOT)")]
    public async Task<IActionResult> AddSensorReading([FromBody] SensorReadingDto readingDto, [FromQuery] int sensorId)
    {
        try
        {
            var sensor = await sensorService.GetByIdAsync(sensorId)
                         ?? throw new KeyNotFoundException($"Sensor with ID {sensorId} not found");
            
            var reading = new SensorReading(
                readingDto.TimeStamp,
                readingDto.Value,
                readingDto.Unit,
                sensor
            );
            
            await sensorService.AddSensorReadingAsync(reading);
            return Ok("Sensor reading added successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    //This endpoint is for getting all latest SensorReadings from each sensor
    [HttpGet("latest/all")]
    public async Task<IActionResult> GetLatestReadingFromAllSensors()
    {
        var readings = await sensorService.GetLatestReadingFromAllSensorsAsync();
        return Ok(readings);
    }
    
    [HttpGet("sensor/readings")]
    public async Task<IActionResult> GetReadingsBySensor()
    {
        var readings = await sensorService.GetReadingsBySensorAsync();
        return Ok(readings);
    }

    [HttpGet("sensor/latest")]
    public async Task<IActionResult> GetLatestReadingBySensor()
    {
        var readings = await sensorService.GetLatestReadingBySensorAsync();
        return Ok(readings);
    }

    [HttpGet("range")]
    public async Task<IActionResult> GetReadingsByTimestampRange([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var readings = await sensorService.GetReadingsByTimestampRangeAsync(start, end);
        return Ok(readings);
    }

    [HttpGet("sensor/{sensorId}/paginated")]
    public async Task<IActionResult> GetPaginatedReadings(int sensorId, [FromQuery] int pageNumber,
        [FromQuery] int pageSize)
    {
        var readings = await sensorService.GetReadingsPaginatedAsync(sensorId, pageNumber, pageSize);
        return Ok(readings);
    }

    [HttpGet("sensor/{sensorId}/average")]
    public async Task<IActionResult> GetAverageReading(int sensorId, [FromQuery] DateTime start,
        [FromQuery] DateTime end)
    {
        var avg = await sensorService.GetAverageReadingForSensorAsync(sensorId, start, end);
        return Ok(avg);
    }
}