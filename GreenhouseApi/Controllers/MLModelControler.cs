using Microsoft.AspNetCore.Mvc;
using Domain.DTOs;
using Domain.Entities;
using Domain.IServices;

namespace GreenhouseApi.Controllers;

[ApiController]
[Route("api/ml")]
public class MlModelController(
    IMlModelService mlModelService,
    ILogger<MlModelController> logger,
    IGreenhouseService greenhouseService)
    : ControllerBase
{
    [HttpPost("predict-next-watering-time/{greenhouseId:int}")]
    public async Task<ActionResult<PredictionResultDto>> PredictNextWateringTime(
        int greenhouseId,
        [FromBody] IEnumerable<SensorReadingDto> input)
    {
        var sensorReadingDtos = input.ToList();
        if (!sensorReadingDtos.Any() || !sensorReadingDtos.Any(h => h.Value > 0))
        {
            logger.LogWarning("Invalid sensor data received for prediction.");
            return BadRequest("Valid historical sensor data are required.");
        }

        try
        {
            var sensors = await greenhouseService.GetSensorsByGreenhouseIdAsync(greenhouseId);

            var sensor = sensors.FirstOrDefault();
            if (sensor == null)
                return BadRequest("No sensors found to associate with readings.");

            var sensorReadings = sensorReadingDtos.Select(dto =>
                new SensorReading(dto.TimeStamp, dto.Value, sensor.Unit, sensor)
            );

            var result = await mlModelService.PredictNextWateringTimeAsync(sensorReadings);

            logger.LogInformation("Prediction successful. Result: {@result}", result);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Validation error during prediction.");
            return BadRequest(Problem(ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during prediction.");
            return StatusCode(500, Problem("An unexpected error occurred while processing the prediction."));
        }
    }
}