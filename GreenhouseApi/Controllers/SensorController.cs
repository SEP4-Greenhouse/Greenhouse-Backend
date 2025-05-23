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
    [HttpPost("/reading(IOT)")]
    public async Task<IActionResult> AddSensorReading([FromBody] SensorReadingDto readingDto, [FromQuery] int sensorId)
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

    [HttpGet("latest/all")]
    public async Task<IActionResult> GetLatestReadingFromAllSensors()
    {
        var readings = await sensorService.GetLatestReadingFromAllSensorsAsync();

        var simplifiedReadings = readings.Select(r => new
        {
            r.TimeStamp,
            r.Value,
            r.Unit,
            r.SensorId
        }).ToList();

        return Ok(simplifiedReadings);
    }

    [HttpGet("/allReadings")]
    public async Task<IActionResult> GetReadingsBySensor()
    {
        var readingsBySensor = await sensorService.GetReadingsBySensorAsync();

        var simplifiedReadings = readingsBySensor.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Select(r => new
            {
                r.TimeStamp,
                r.Value,
                r.Unit,
                r.SensorId,
            }).ToList()
        );

        return Ok(simplifiedReadings);
    }

    [HttpGet("{sensorId}/latestBySensor")]
    public async Task<IActionResult> GetLatestReadingBySensor(int sensorId)
    {
        var latestReadings = await sensorService.GetLatestReadingBySensorAsync();
        if (!latestReadings.TryGetValue(sensorId, out var latestReading))
            return NotFound($"No latest reading found for sensor {sensorId}.");

        var simplifiedReading = new
        {
            latestReading.TimeStamp,
            latestReading.Value,
            latestReading.Unit,
        };

        return Ok(simplifiedReading);
    }

    [HttpGet("range")]
    public async Task<IActionResult> GetReadingsByTimestampRange([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var startUtc = DateTime.SpecifyKind(start, DateTimeKind.Utc);
        var endUtc = DateTime.SpecifyKind(end, DateTimeKind.Utc);

        var readings = await sensorService.GetReadingsByTimestampRangeAsync(startUtc, endUtc);

        var simplifiedReadings = readings.Select(r => new
        {
            r.TimeStamp,
            r.Value,
            r.Unit,
            r.SensorId
        }).ToList();

        return Ok(simplifiedReadings);
    }

    [HttpGet("/{sensorId}/paginated")]
    public async Task<IActionResult> GetPaginatedReadings(int sensorId, [FromQuery] int pageNumber,
        [FromQuery] int pageSize)
    {
        var readings = await sensorService.GetReadingsPaginatedAsync(sensorId, pageNumber, pageSize);

        var simplifiedReadings = readings.Select(r => new
        {
            r.TimeStamp,
            r.Value,
            r.Unit,
        }).ToList();

        return Ok(simplifiedReadings);
    }

    [HttpGet("/{sensorId}/average")]
    public async Task<IActionResult> GetAverageReading(int sensorId, [FromQuery] DateTime start,
        [FromQuery] DateTime end)
    {
        var startUtc = DateTime.SpecifyKind(start, DateTimeKind.Utc);
        var endUtc = DateTime.SpecifyKind(end, DateTimeKind.Utc);

        var avg = await sensorService.GetAverageReadingForSensorAsync(sensorId, startUtc, endUtc);

        var sensor = await sensorService.GetByIdAsync(sensorId);

        return Ok(new
        {
            Average = avg,
            sensor.Unit,
            SensorId = sensorId,
            TimeRange = new { Start = start, End = end }
        });
    }

    [HttpPost("/{sensorId}/threshold")]
    public async Task<IActionResult> AddThresholdToSensor(int sensorId, [FromBody] ThresholdDto thresholdDto)
    {
        var threshold = await sensorService.AddThresholdToSensorAsync(sensorId, thresholdDto);
        return Ok(new
        {
            threshold.MinValue,
            threshold.MaxValue
        });
    }

    [HttpGet("/{sensorId}/threshold")]
    public async Task<IActionResult> GetThresholdFromSensor(int sensorId)
    {
        var threshold = await sensorService.GetThresholdBySensorIdAsync(sensorId);
        if (threshold == null)
            return NotFound($"Threshold for sensor {sensorId} not found.");

        return Ok(new
        {
            threshold.MinValue,
            threshold.MaxValue
        });
    }

    [HttpPut("/{sensorId}/threshold")]
    public async Task<IActionResult> UpdateThreshold(int sensorId, [FromBody] ThresholdDto thresholdDto)
    {
        var threshold = await sensorService.GetThresholdBySensorIdAsync(sensorId);
        if (threshold == null)
            return NotFound($"Threshold for sensor {sensorId} not found.");

        threshold.MinValue = thresholdDto.MinValue;
        threshold.MaxValue = thresholdDto.MaxValue;

        var updatedThreshold = await sensorService.UpdateThresholdAsync(threshold);

        return Ok(new
        {
            updatedThreshold.MinValue,
            updatedThreshold.MaxValue
        });
    }

    [HttpDelete("/{sensorId}/threshold")]
    public async Task<IActionResult> DeleteThreshold(int sensorId)
    {
        var deleted = await sensorService.DeleteThresholdAsync(sensorId);
        if (!deleted)
            return NotFound($"Threshold for sensor {sensorId} not found.");

        return Ok($"Threshold for sensor {sensorId} deleted successfully.");
    }
}