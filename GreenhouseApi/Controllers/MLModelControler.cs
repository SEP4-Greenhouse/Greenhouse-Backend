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
        if (input == null || string.IsNullOrWhiteSpace(input.SensorType))
        {
            _logger.LogWarning("Invalid input received for prediction.");
            return BadRequest("Invalid input data.");
        }

        try
        {
            var result = await _mlModelService.PredictAsync(input);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while processing prediction.");
            return StatusCode(500, "An error occurred while processing the prediction.");
        }
    }

    // GET /api/ml/latest-data
    [HttpGet("latest-data")]
    public ActionResult<IEnumerable<SensorDataDto>> GetLatestSensorData()
    {
        try
        {
            var mockData = new List<SensorDataDto>
            {
                new SensorDataDto("Temperature", 26.8, DateTime.UtcNow),
                new SensorDataDto("Humidity", 34.5, DateTime.UtcNow)
            };

            return Ok(mockData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching latest sensor data.");
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