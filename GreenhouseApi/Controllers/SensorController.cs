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
            
            var utcTimestamp = DateTime.SpecifyKind(readingDto.TimeStamp, DateTimeKind.Utc);
            
            var reading = new SensorReading(
                utcTimestamp,
                readingDto.Value,
                sensor.Unit,
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
    
        var simplifiedReadings = readings.Select(r => new
        {
            Id = r.Id,
            TimeStamp = r.TimeStamp,
            Value = r.Value,
            Unit = r.Unit,
            SensorId = r.SensorId
        }).ToList();
    
        return Ok(simplifiedReadings);
    }
    
    [HttpGet("sensor/readings")]
    public async Task<IActionResult> GetReadingsBySensor()
    {
        var readingsBySensor = await sensorService.GetReadingsBySensorAsync();
    
        var simplifiedReadings = readingsBySensor.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Select(r => new
            {
                Id = r.Id,
                TimeStamp = r.TimeStamp,
                Value = r.Value,
                Unit = r.Unit
            }).ToList()
        );
    
        return Ok(simplifiedReadings);
    }

    [HttpGet("sensor/latest")]
    public async Task<IActionResult> GetLatestReadingBySensor()
    {
        var latestReadings = await sensorService.GetLatestReadingBySensorAsync();
    
        var simplifiedReadings = latestReadings.ToDictionary(
            kvp => kvp.Key,
            kvp => new
            {
                Id = kvp.Value.Id,
                TimeStamp = kvp.Value.TimeStamp,
                Value = kvp.Value.Value,
                Unit = kvp.Value.Unit
            }
        );
    
        return Ok(simplifiedReadings);
    }

    [HttpGet("range")]
    public async Task<IActionResult> GetReadingsByTimestampRange([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        // Convert dates to UTC
        var startUtc = DateTime.SpecifyKind(start, DateTimeKind.Utc);
        var endUtc = DateTime.SpecifyKind(end, DateTimeKind.Utc);
        
        var readings = await sensorService.GetReadingsByTimestampRangeAsync(startUtc, endUtc);
    
        var simplifiedReadings = readings.Select(r => new
        {
            Id = r.Id,
            TimeStamp = r.TimeStamp,
            Value = r.Value,
            Unit = r.Unit,
            SensorId = r.SensorId
        }).ToList();
    
        return Ok(simplifiedReadings);
    }

    [HttpGet("sensor/{sensorId}/paginated")]
    public async Task<IActionResult> GetPaginatedReadings(int sensorId, [FromQuery] int pageNumber,
        [FromQuery] int pageSize)
    {
        var readings = await sensorService.GetReadingsPaginatedAsync(sensorId, pageNumber, pageSize);
    
        var simplifiedReadings = readings.Select(r => new
        {
            Id = r.Id,
            TimeStamp = r.TimeStamp,
            Value = r.Value,
            Unit = r.Unit
        }).ToList();
    
        return Ok(simplifiedReadings);
    }

    [HttpGet("sensor/{sensorId}/average")]
    public async Task<IActionResult> GetAverageReading(int sensorId, [FromQuery] DateTime start,
        [FromQuery] DateTime end)
    {
        try
        {
            var startUtc = DateTime.SpecifyKind(start, DateTimeKind.Utc);
            var endUtc = DateTime.SpecifyKind(end, DateTimeKind.Utc);
            
            var avg = await sensorService.GetAverageReadingForSensorAsync(sensorId, startUtc, endUtc);
            
            var sensor = await sensorService.GetByIdAsync(sensorId);
            if (sensor == null)
                return NotFound($"Sensor with ID {sensorId} not found");

            return Ok(new {
                Average = avg,
                Unit = sensor.Unit,
                SensorId = sensorId,
                TimeRange = new { Start = start, End = end }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}