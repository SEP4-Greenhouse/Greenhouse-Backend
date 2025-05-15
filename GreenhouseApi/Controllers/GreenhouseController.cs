using Domain.DTOs;
using Domain.Entities;
using Domain.IServices;
using Microsoft.AspNetCore.Mvc;

namespace GreenhouseApi.Controllers;

[ApiController]
[Route("api/greenhouse")]
public class GreenhouseController(IGreenhouseService greenhouseService, IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllGreenhouses()
    {
        try
        {
            var greenhouses = await greenhouseService.GetAllAsync();
            return Ok(greenhouses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGreenhouseById(int id)
    {
        try
        {
            var greenhouse = await greenhouseService.GetByIdAsync(id);
            return Ok(greenhouse);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetGreenhousesByUserId(int userId)
    {
        try
        {
            var greenhouses = await greenhouseService.GetGreenhousesByUserIdAsync(userId);
            return Ok(greenhouses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateGreenhouse([FromBody] GreenhouseCreateDto dto)
    {
        try
        {
            var user = await userService.GetUserByIdAsync(dto.UserId);
            var greenhouse = new Greenhouse(dto.PlantType, user);
            var result = await greenhouseService.AddAsync(greenhouse);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}/plant-type")]
    public async Task<IActionResult> UpdatePlantType(int id, [FromBody] string newPlantType)
    {
        try
        {
            await greenhouseService.UpdatePlantTypeAsync(id, newPlantType);
            return Ok("Plant type updated successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGreenhouse(int id)
    {
        try
        {
            await greenhouseService.DeleteAsync(id);
            return Ok("Greenhouse deleted successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}/sensors")]
    public async Task<IActionResult> GetGreenhouseSensors(int id)
    {
        try
        {
            var sensors = await greenhouseService.GetSensorsByGreenhouseIdAsync(id);
            return Ok(sensors);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}/actuators")]
    public async Task<IActionResult> GetGreenhouseActuators(int id)
    {
        try
        {
            var actuators = await greenhouseService.GetActuatorsByGreenhouseIdAsync(id);
            return Ok(actuators);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}/plants")]
    public async Task<IActionResult> GetGreenhousePlants(int id)
    {
        try
        {
            var plants = await greenhouseService.GetPlantsByGreenhouseIdAsync(id);
            return Ok(plants);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{id}/sensors")]
    public async Task<IActionResult> AddSensorToGreenhouse(int id, [FromBody] CreateSensorDTO sensorDto)
    {
        try
        {
            var greenhouse = await greenhouseService.GetByIdAsync(id);
            // Don't specify ID - let database generate it
            var sensor = new Sensor( sensorDto.Type, sensorDto.Status, greenhouse);
            await greenhouseService.AddSensorToGreenhouseAsync(id, sensor);
            return Ok("Sensor added to greenhouse successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/actuators")]
    public async Task<IActionResult> AddActuatorToGreenhouse(int id, [FromBody] Actuator actuator)
    {
        try
        {
            await greenhouseService.AddActuatorToGreenhouseAsync(id, actuator);
            return Ok("Actuator added to greenhouse successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/plants")]
    public async Task<IActionResult> AddPlantToGreenhouse(int id, [FromBody] Plant plant)
    {
        try
        {
            await greenhouseService.AddPlantToGreenhouseAsync(id, plant);
            return Ok("Plant added to greenhouse successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}