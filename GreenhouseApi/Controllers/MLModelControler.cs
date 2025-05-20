// using Microsoft.AspNetCore.Mvc;
// using Domain.DTOs;
// using Domain.Entities;
// using Domain.IRepositories;
// using Domain.IServices;
//
// namespace GreenhouseApi.Controllers;
//
// // API route: /api/ml
// [ApiController]
// [Route("api/ml")]
// public class MlModelController(
//     IMlModelService mlModelService,               // Service to handle prediction logic
//     IPredictionLogRepository logRepo,             // Repository to save prediction logs
//     ILogger<MlModelController> logger)            // Logger for tracking events and errors
//     : ControllerBase
// {
//     // POST /api/ml/predict
//     [HttpPost("predict")]
//     public async Task<ActionResult<PredictionResultDto>> Predict([FromBody] SensorDataDto input)
//     {
//         // Validate input
//         if (input == null || input.current == null || input.history == null || !input.history.Any())
//         {
//             logger.LogWarning("Invalid sensor data received for prediction.");
//             return BadRequest("Current and history sensor data are required.");
//         }
//
//         try
//         {
//             // Call ML service to make a prediction
//             var result = await mlModelService.PredictAsync(input);
//
//             if (result == null)
//             {
//                 logger.LogWarning("Prediction result is null.");
//                 return StatusCode(500, "Prediction service returned no result.");
//             }
//
//             return Ok(result); // Return the prediction
//         }
//         catch (ArgumentException ex)
//         {
//             logger.LogError(ex, "Validation error occurred while processing prediction.");
//             return BadRequest(ex.Message); // Handle known errors gracefully
//         }
//         catch (Exception ex)
//         {
//             logger.LogError(ex, "Unexpected error occurred while processing the prediction.");
//             return StatusCode(500, "An error occurred while processing the prediction.");
//         }
//     }
//
//     // POST /api/ml/logs
//     [HttpPost("logs")]
//     public async Task<IActionResult> LogPrediction([FromBody] PredictionLog log)
//     {
//         // Validate input
//         if (log == null)
//         {
//             logger.LogWarning("Null log received.");
//             return BadRequest("Log data is required.");
//         }
//
//         try
//         {
//             // Save log to database
//             await logRepo.AddAsync(log);
//             return Ok("Prediction log saved successfully.");
//         }
//         catch (Exception ex)
//         {
//             logger.LogError(ex, "Error saving prediction log.");
//             return StatusCode(500, "An error occurred while saving the prediction log.");
//         }
//     }
// }
