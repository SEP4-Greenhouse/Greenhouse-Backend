using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
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

    [HttpPost("predict")]
    public async Task<ActionResult<PredictionLog>> Predict([FromBody] SensorDataDto input)
    {
        if (input == null || input.Current == null || string.IsNullOrWhiteSpace(input.Current.SensorType))
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
}