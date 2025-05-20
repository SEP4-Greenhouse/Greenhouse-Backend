using Domain.DTOs;
using Domain.Entities;
using Domain.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenhouseApi.Controllers;
[Authorize]
[ApiController]
[Route("api/sensor")]
public class SensorController(ISensorService sensorService) : ControllerBase
{
    
    // This Endpoint is for IOT Team to add a new sensor reading into the database.
    [HttpPost("readings(IOT)")]
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
    [HttpGet("latest/eachSensor")]
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
    
    [HttpGet("all")]
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

    [HttpGet("{sensorId}/allPerSensor")]
    public async Task<IActionResult> GetAllReadingsBySensor(int sensorId)
    {
        try
        {
            var sensor = await sensorService.GetByIdAsync(sensorId)
                         ?? throw new KeyNotFoundException($"Sensor with ID {sensorId} not found");
            
            var readingsBySensor = await sensorService.GetReadingsBySensorAsync();
            
            if (!readingsBySensor.TryGetValue(sensorId, out var readings) || !readings.Any())
            {
                return NotFound($"No readings found for sensor with ID {sensorId}");
            }
            
            var simplifiedReadings = readings.Select(r => new
            {
                Id = r.Id,
                TimeStamp = r.TimeStamp,
                Value = r.Value,
                Unit = r.Unit
            }).ToList();

            return Ok(simplifiedReadings);
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
    
    [HttpGet("{sensorId}/latestPerSensor")]
    public async Task<IActionResult> GetLatestReadingBySensor(int sensorId)
    {
        try
        {
            var sensor = await sensorService.GetByIdAsync(sensorId)
                         ?? throw new KeyNotFoundException($"Sensor with ID {sensorId} not found");
            
            var latestReadings = await sensorService.GetLatestReadingBySensorAsync();
            
            if (!latestReadings.TryGetValue(sensorId, out var reading))
            {
                return NotFound($"No readings found for sensor with ID {sensorId}");
            }
            
            var simplifiedReading = new
            {
                Id = reading.Id,
                TimeStamp = reading.TimeStamp,
                Value = reading.Value,
                Unit = reading.Unit
            };
        
            return Ok(simplifiedReading);
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

    [HttpGet("byRange")]
    public async Task<IActionResult> GetReadingsByTimestampRange([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
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
}