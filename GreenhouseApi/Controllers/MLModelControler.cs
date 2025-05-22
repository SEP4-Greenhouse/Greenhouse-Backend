using Microsoft.AspNetCore.Mvc;
using Domain.DTOs;
using Domain.Entities;
using Domain.IServices;
using Microsoft.AspNetCore.Authorization;

namespace GreenhouseApi.Controllers;

[Authorize]
[ApiController]
[Route("api/ml")]
public class MlModelController(IMlModelService mlModelService) : ControllerBase
{
    [HttpPost("predict-next-watering-time/{plantId:int}")]
    public async Task<ActionResult<PredictionResultDto>> PredictNextWateringTime(int plantId)
    {
        var mlData = new MlModelDataDto();
        await mlModelService.PrepareDataForPredictionAsync(mlData, plantId);

        var result = await mlModelService.PredictNextWateringTimeAsync(mlData, plantId);
        return Ok(result);
    }

    [HttpGet("prediction-logs")]
    public async Task<ActionResult<IEnumerable<PredictionLog>>> GetAllPredictionLogs()
    {
        var logs = await mlModelService.GetAllPredictionLogsAsync();
        return Ok(logs);
    }
}