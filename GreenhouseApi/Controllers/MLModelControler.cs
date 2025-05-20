/*
using Microsoft.AspNetCore.Mvc;
using Domain.DTOs;
using Domain.Entities;
using Domain.IRepositories;
using Domain.IServices;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace GreenhouseApi.Controllers;

[ApiController]
[Route("api/ml")]
public class MlModelController(
    IMlModelService mlModelService,
    IPredictionLogRepository logRepo,
    ILogger<MlModelController> logger)
    : ControllerBase
{
    // POST /api/ml/predict
    [HttpPost("predict")]
    public async Task<ActionResult<PredictionResultDto>> Predict([FromBody] JObject payload)
    {
        try
        {
            // Deserialize each component manually
            var sensor = payload["sensor"]?.ToObject<SensorDto>();
            var current = payload["current"]?.ToObject<SensorReadingDto>();
            var history = payload["history"]?.ToObject<List<SensorReadingDto>>();

            // Validate presence of all components
            if (sensor == null || current == null || history == null || !history.Any())
            {
                logger.LogWarning("Invalid sensor data received for prediction.");
                return BadRequest("Sensor, current, and history data are required.");
            }

            // Manually construct a temporary object or anonymous structure
            var result = await mlModelService.PredictAsync(new
            {
                Sensor = sensor,
                Current = current,
                History = history
            });

            if (result == null)
            {
                logger.LogWarning("Prediction result is null.");
                return StatusCode(500, "Prediction service returned no result.");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while processing the prediction.");
            return StatusCode(500, "An error occurred while processing the prediction.");
        }
    }

    // POST /api/ml/logs
    [HttpPost("logs")]
    public async Task<IActionResult> LogPrediction([FromBody] PredictionLog log)
    {
        if (log == null)
        {
            logger.LogWarning("Null log received.");
            return BadRequest("Log data is required.");
        }

        try
        {
            await logRepo.AddAsync(log);
            return Ok("Prediction log saved successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error saving prediction log.");
            return StatusCode(500, "An error occurred while saving the prediction log.");
        }
    }
}
*/
