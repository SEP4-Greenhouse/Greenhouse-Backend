using Microsoft.AspNetCore.Mvc;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MLModelClient.Services;

namespace GreenhouseApi.Controllers;

[ApiController]
[Route("api/ml")]
public class MlModelController : ControllerBase
{
    private readonly IMlModelService _mlModelService;
    private readonly IPredictionLogRepository _logRepo;

    public MlModelController(IMlModelService mlModelService, IPredictionLogRepository logRepo)
    {
        _mlModelService = mlModelService;
        _logRepo = logRepo;
    }

    // POST /api/ml/predict
    [HttpPost("predict")]
    public async Task<ActionResult<PredictionResultDto>> Predict([FromBody] SensorDataDto input)
    {
        var result = await _mlModelService.PredictAsync(input);
        return Ok(result);
    }

    // GET /api/ml/latest-data (mock)
    [HttpGet("latest-data")]
    public ActionResult<IEnumerable<SensorDataDto>> GetLatestSensorData()
    {
        var mockData = new List<SensorDataDto>
        {
            new SensorDataDto
            {
                SensorType = "Temperature",
                Value = 26.8,
                Timestamp = DateTime.UtcNow
            },
            new SensorDataDto
            {
                SensorType = "Humidity",
                Value = 34.5,
                Timestamp = DateTime.UtcNow
            }
        };

        return Ok(mockData);
    }

    // ✅ NEW: GET /api/ml/logs
    [HttpGet("logs")]
    public async Task<ActionResult<IEnumerable<PredictionLog>>> GetAllLogs()
    {
        var logs = await _logRepo.GetAllAsync();
        return Ok(logs);
    }
}