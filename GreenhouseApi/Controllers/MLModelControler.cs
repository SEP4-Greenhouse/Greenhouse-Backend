using Microsoft.AspNetCore.Mvc;
using Domain.DTOs;
using Domain.Entities;
using Domain.IRepositories;
using Domain.IServices;

namespace GreenhouseApi.Controllers;

[ApiController]
[Route("api/ml")]
public class MlModelController : ControllerBase
{
    private readonly IMlModelService _mlModelService;
    private readonly IPredictionLogRepository _logRepo;
    private readonly ILogger<MlModelController> _logger;

    public MlModelController(IMlModelService mlModelService, IPredictionLogRepository logRepo, ILogger<MlModelController> logger)
    {
        _mlModelService = mlModelService;
        _logRepo = logRepo;
        _logger = logger;
    }

    // POST /api/ml/predict
    [HttpPost("predict")]
    public async Task<ActionResult<PredictionResultDto>> Predict([FromBody] SensorDataDto input)
    {
        if (input == null || input.current == null || input.history == null || !input.history.Any())
        {
            _logger.LogWarning("Invalid sensor data received for prediction.");
            return BadRequest("Current and history sensor data are required.");
        }

        try
        {
            var result = await _mlModelService.PredictAsync(input);

            if (result == null)
            {
                _logger.LogWarning("Prediction result is null.");
                return StatusCode(500, "Prediction service returned no result.");
            }

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Validation error occurred while processing prediction.");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while processing the prediction.");
            return StatusCode(500, "An error occurred while processing the prediction.");
        }
    }

    // GET /api/ml/latest-data
    [HttpGet("latest-data")]
    public ActionResult<IEnumerable<SensorDataDto>> GetLatestSensorData()
    {
        try
        {
            // Replace mock data with real sensor data fetching logic
            var mockData = new List<SensorDataDto>
            {
                new SensorDataDto
                {
                    current = new SensorData
                    {
                        sensorType = "Temperature",
                        value = 26.8f,
                        timestamp = DateTime.UtcNow
                    },
                    history = new List<SensorData>
                    {
                        new SensorData
                        {
                            sensorType = "Temperature",
                            value = 26.0f,
                            timestamp = DateTime.UtcNow.AddMinutes(-5)
                        },
                        new SensorData
                        {
                            sensorType = "Temperature",
                            value = 25.5f,
                            timestamp = DateTime.UtcNow.AddMinutes(-10)
                        }
                    }
                },
                new SensorDataDto
                {
                    current = new SensorData
                    {
                        sensorType = "Humidity",
                        value = 34.5f,
                        timestamp = DateTime.UtcNow
                    },
                    history = new List<SensorData>
                    {
                        new SensorData
                        {
                            sensorType = "Humidity",
                            value = 33.0f,
                            timestamp = DateTime.UtcNow.AddMinutes(-5)
                        },
                        new SensorData
                        {
                            sensorType = "Humidity",
                            value = 32.5f,
                            timestamp = DateTime.UtcNow.AddMinutes(-10)
                        }
                    }
                }
            };

            return Ok(mockData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching the latest sensor data.");
            return StatusCode(500, "An error occurred while fetching the latest sensor data.");
        }
    }

    // GET /api/ml/logs
    [HttpGet("logs")]
    public async Task<ActionResult<IEnumerable<PredictionLog>>> GetAllLogs()
    {
        try
        {
            var logs = await _logRepo.GetAllAsync();
            return Ok(logs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching prediction logs.");
            return StatusCode(500, "An error occurred while fetching prediction logs.");
        }
    }
}
