using Domain.DTOs;
using Domain.Entities;
using Domain.Entities.Actuators;
using Domain.IServices;
using Microsoft.AspNetCore.Mvc;

namespace GreenhouseApi.Controllers;

[ApiController]
[Route("api/greenhouse")]
public class GreenhouseController(IGreenhouseService greenhouseService, IUserService userService) : ControllerBase
{
    // GREENHOUSE ENDPOINTS
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetGreenhousesByUserId(int userId)
    {
        try
        {
            var greenhouses = await greenhouseService.GetGreenhousesByUserIdAsync(userId);
        
            var simplifiedGreenhouses = greenhouses.Select(g => new
            {
                Id = g.Id,
                Name = g.Name,
                PlantType = g.PlantType
            }).ToList();
        
            return Ok(simplifiedGreenhouses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateGreenhouse([FromBody] GreenhouseDto dto)
    {
        try
        {
            var user = await userService.GetUserByIdAsync(dto.UserId);
            var greenhouse = new Greenhouse(dto.Name, dto.PlantType, dto.UserId);
            var result = await greenhouseService.AddAsync(greenhouse);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id}/name")]
    public async Task<IActionResult> UpdateGreenhouseName(int id, [FromBody] string newName)
    {
        try
        {
            var greenhouse = await greenhouseService.GetByIdAsync(id);
            if (greenhouse == null)
                return NotFound($"Greenhouse with ID {id} not found");

            greenhouse.UpdateName(newName);
            await greenhouseService.UpdateAsync(greenhouse);
            return Ok("Greenhouse name updated successfully.");
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

    [HttpPut("{id}/planttype")]
    public async Task<IActionResult> UpdateGreenhousePlantType(int id, [FromBody] string newPlantType)
    {
        try
        {
            var greenhouse = await greenhouseService.GetByIdAsync(id);
            if (greenhouse == null)
                return NotFound($"Greenhouse with ID {id} not found");

            greenhouse.UpdatePlantType(newPlantType);
            await greenhouseService.UpdateAsync(greenhouse);
            return Ok("Greenhouse plant type updated successfully.");
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

    // PLANT ENDPOINTS
    [HttpGet("{id}/plants")]
    public async Task<IActionResult> GetGreenhousePlants(int id)
    {
        try
        {
            var plants = await greenhouseService.GetPlantsByGreenhouseIdAsync(id);
        
            var simplifiedPlants = plants.Select(p => new
            {
                Id = p.Id,
                Species = p.Species,
                PlantingDate = p.PlantingDate,
                GrowthStage = p.GrowthStage
            }).ToList();
        
            return Ok(simplifiedPlants);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{id}/plants")]
    public async Task<IActionResult> AddPlantToGreenhouse(int id, [FromBody] PlantDto plantDto)
    {
        try
        {
            // First get the greenhouse by id
            var greenhouse = await greenhouseService.GetByIdAsync(id);

            // Create a new Plant object from the DTO data
            var plant = new Plant(
                plantDto.Species,
                plantDto.PlantingDate,
                plantDto.GrowthStage,
                greenhouse
            );

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

    [HttpPut("{greenhouseId}/plantsGrowingStage/{plantId}")]
    public async Task<IActionResult> UpdatePlantInGreenhouse(int greenhouseId, int plantId, [FromBody] PlantDto plantDto)
    {
        try
        {
            var existingPlant = await greenhouseService.GetPlantsByGreenhouseIdAsync(greenhouseId)
                .ContinueWith(t => t.Result.FirstOrDefault(p => p.Id == plantId));
            
            if (existingPlant == null)
                return NotFound($"Plant with ID {plantId} not found in greenhouse {greenhouseId}");
            
            existingPlant.UpdateGrowthStage(plantDto.GrowthStage);
            
            await greenhouseService.UpdatePlantInGreenhouseAsync(greenhouseId, plantId, existingPlant);
            return Ok("Plant updated successfully.");
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
    
    [HttpPut("{greenhouseId}/plantsSpecies/{plantId}")]
    public async Task<IActionResult> UpdatePlantSpeciesInGreenhouse(int greenhouseId, int plantId, [FromBody] PlantDto plantDto)
    {
        try
        {
            var existingPlant = await greenhouseService.GetPlantsByGreenhouseIdAsync(greenhouseId)
                .ContinueWith(t => t.Result.FirstOrDefault(p => p.Id == plantId));
            
            if (existingPlant == null)
                return NotFound($"Plant with ID {plantId} not found in greenhouse {greenhouseId}");
            
            existingPlant.UpdateSpecies(plantDto.Species);
            
            await greenhouseService.UpdatePlantInGreenhouseAsync(greenhouseId, plantId, existingPlant);
            return Ok("Plant updated successfully.");
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
    
    [HttpDelete("{greenhouseId}/plants/{plantId}")]
    public async Task<IActionResult> DeletePlantFromGreenhouse(int greenhouseId, int plantId)
    {
        try
        {
            await greenhouseService.DeletePlantFromGreenhouseAsync(greenhouseId, plantId);
            return Ok("Plant deleted from greenhouse successfully.");
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
    
    // SENSOR ENDPOINTS
    [HttpGet("{id}/sensors")]
    public async Task<IActionResult> GetGreenhouseSensors(int id)
    {
        try
        {
            var sensors = await greenhouseService.GetSensorsByGreenhouseIdAsync(id);
        
            var simplifiedSensors = sensors.Select(s => new
            {
                Id = s.Id,
                Type = s.Type,
                Status = s.Status
            }).ToList();
        
            return Ok(simplifiedSensors);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpPost("{id}/sensors")]
    public async Task<IActionResult> AddSensorToGreenhouse(int id, [FromBody] SensorDto sensorDto)
    {
        try
        {
            var greenhouse = await greenhouseService.GetByIdAsync(id);
            var sensor = new Sensor(sensorDto.Type, sensorDto.Status, sensorDto.Unit, greenhouse);
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
    
    [HttpPut("{greenhouseId}/sensors/{sensorId}/status")]
    public async Task<IActionResult> UpdateSensorStatus(int greenhouseId, int sensorId, [FromBody] string newStatus)
    {
        try
        {
            var existingSensor = await greenhouseService.GetSensorsByGreenhouseIdAsync(greenhouseId)
                .ContinueWith(t => t.Result.FirstOrDefault(s => s.Id == sensorId));

            if (existingSensor == null)
                return NotFound($"Sensor with ID {sensorId} not found in greenhouse {greenhouseId}");

            existingSensor.UpdateStatus(newStatus);

            await greenhouseService.UpdateSensorInGreenhouseAsync(greenhouseId, sensorId, existingSensor);
            return Ok("Sensor status updated successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
    
    [HttpDelete("{greenhouseId}/sensors/{sensorId}")]
    public async Task<IActionResult> DeleteSensorFromGreenhouse(int greenhouseId, int sensorId)
    {
        try
        {
            await greenhouseService.DeleteSensorFromGreenhouseAsync(greenhouseId, sensorId);
            return Ok("Sensor deleted from greenhouse successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
    
    // ACTUATOR ENDPOINTS
    [HttpGet("{id}/actuators")]
    public async Task<IActionResult> GetGreenhouseActuators(int id)
    {
        try
        {
            var actuators = await greenhouseService.GetActuatorsByGreenhouseIdAsync(id);
        
            var simplifiedActuators = actuators.Select(a => new
            {
                Id = a.Id,
                Type = a.GetType().Name.Replace("Actuator", ""),
                Status = a.Status
            }).ToList();
        
            return Ok(simplifiedActuators);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPost("{id}/actuators")]
    public async Task<IActionResult> AddActuatorToGreenhouse(int id, [FromBody] ActuatorDto actuatorDto)
    {
        try
        {
            var greenhouse = await greenhouseService.GetByIdAsync(id);

            Actuator actuator = actuatorDto.Type.ToLower() switch
            {
                "waterpump" => new WaterPumpActuator(actuatorDto.Status, greenhouse),
                "servomotor" => new ServoMotorActuator(actuatorDto.Status, greenhouse),
                _ => throw new ArgumentException($"Unsupported actuator type: {actuatorDto.Type}")
            };

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
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
    
    [HttpPut("{greenhouseId}/actuators/{actuatorId}/status")]
    public async Task<IActionResult> UpdateActuatorStatus(int greenhouseId, int actuatorId, [FromBody] string newStatus)
    {
        try
        {
            var existingActuator = await greenhouseService.GetActuatorsByGreenhouseIdAsync(greenhouseId)
                .ContinueWith(t => t.Result.FirstOrDefault(a => a.Id == actuatorId));
        
            if (existingActuator == null)
                return NotFound($"Actuator with ID {actuatorId} not found in greenhouse {greenhouseId}");
    
            existingActuator.UpdateStatus(newStatus);
    
            await greenhouseService.UpdateActuatorInGreenhouseAsync(greenhouseId, actuatorId, existingActuator);
            return Ok("Actuator status updated successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Unexpected error: {ex.Message}");
        }
    }
    
    [HttpDelete("{greenhouseId}/actuators/{actuatorId}")]
    public async Task<IActionResult> DeleteActuatorFromGreenhouse(int greenhouseId, int actuatorId)
    {
        try
        {
            await greenhouseService.DeleteActuatorFromGreenhouseAsync(greenhouseId, actuatorId);
            return Ok("Actuator deleted from greenhouse successfully.");
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