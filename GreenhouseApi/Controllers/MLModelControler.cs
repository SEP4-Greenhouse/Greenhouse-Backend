using Microsoft.AspNetCore.Mvc;
using Domain.DTOs;
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
        try
        {
            var mlData = new MlModelDataDto();
            await mlModelService.PrepareDataForPredictionAsync(mlData, plantId);

            var result = await mlModelService.PredictNextWateringTimeAsync(mlData);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(Problem(ex.Message));
        }
        catch (Exception ex)
        {
            // Log the exception (add your logger here)
            Console.WriteLine(ex); // Replace with proper logging in production
            return StatusCode(500, Problem($"An unexpected error occurred: {ex.Message}"));
        }
    }
}