using Domain.DTOs;
using Domain.Entities;
using Domain.Entities.Actuators;
using Domain.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenhouseApi.Controllers;

[Authorize]
[ApiController]
[Route("api/greenhouse")]
public class GreenhouseController(IGreenhouseService greenhouseService, IUserService userService)
    : ControllerBase
{
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetGreenhousesByUserId(int userId)
    {
        var greenhouses = await greenhouseService.GetGreenhousesByUserIdAsync(userId);

        var simplifiedGreenhouses = greenhouses.Select(g => new
        {
            g.Id,
            g.Name,
            g.PlantType
        }).ToList();

        return Ok(simplifiedGreenhouses);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGreenhouse([FromBody] GreenhouseDto dto)
    {
        var user = await userService.GetUserByIdAsync(dto.UserId);
        if (dto is { Name: not null, PlantType: not null } && user != null)
        {
            var greenhouse = new Greenhouse(dto.Name, dto.PlantType, dto.UserId);
            await greenhouseService.AddAsync(greenhouse);
        }

        return Ok();
    }

    [HttpPut("{id}/name")]
    public async Task<IActionResult> UpdateGreenhouseName(int id, [FromBody] string newName)
    {
        var greenhouse = await greenhouseService.GetByIdAsync(id);

        greenhouse.UpdateName(newName);
        await greenhouseService.UpdateAsync(greenhouse);
        return Ok("Greenhouse name updated successfully.");
    }

    [HttpPut("{id}/plantType")]
    public async Task<IActionResult> UpdateGreenhousePlantType(int id, [FromBody] string newPlantType)
    {
        var greenhouse = await greenhouseService.GetByIdAsync(id);

        greenhouse.UpdatePlantType(newPlantType);
        await greenhouseService.UpdateAsync(greenhouse);
        return Ok("Greenhouse plant type updated successfully.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGreenhouse(int id)
    {
        await greenhouseService.DeleteAsync(id);
        return Ok("Greenhouse deleted successfully.");
    }

    // PLANT ENDPOINTS
    [HttpGet("{id}/plants")]
    public async Task<IActionResult> GetGreenhousePlants(int id)
    {
        var plants = await greenhouseService.GetPlantsByGreenhouseIdAsync(id);

        var simplifiedPlants = plants.Select(p => new
        {
            p.Id,
            p.Species,
            p.PlantingDate,
            p.GrowthStage
        }).ToList();

        return Ok(simplifiedPlants);
    }

    [HttpPost("{id}/plants")]
    public async Task<IActionResult> AddPlantToGreenhouse(int id, [FromBody] PlantDto plantDto)
    {
        var greenhouse = await greenhouseService.GetByIdAsync(id);

        var plant = new Plant(
            plantDto.Species,
            plantDto.PlantingDate,
            plantDto.GrowthStage,
            greenhouse
        );

        var created = await greenhouseService.AddPlantToGreenhouseAsync(id, plant);

        return Ok(new
        {
            created.Id,
            created.Species,
            created.PlantingDate,
            created.GrowthStage
        });
    }


    [HttpPut("{greenhouseId}/plants/{plantId}/growthStage")]
    public async Task<IActionResult> UpdatePlantGrowthStage(int greenhouseId, int plantId,
        [FromBody] string newGrowthStage)
    {
        var plants = await greenhouseService.GetPlantsByGreenhouseIdAsync(greenhouseId);
        var existingPlant = plants.FirstOrDefault(p => p.Id == plantId);

        if (existingPlant == null)
            return NotFound($"Plant with ID {plantId} not found in greenhouse {greenhouseId}");

        existingPlant.UpdateGrowthStage(newGrowthStage);
        await greenhouseService.UpdatePlantInGreenhouseAsync(greenhouseId, plantId, existingPlant);
        return Ok("Plant growth stage updated successfully.");
    }

    [HttpPut("{greenhouseId}/plants/{plantId}/species")]
    public async Task<IActionResult> UpdatePlantSpecies(int greenhouseId, int plantId, [FromBody] string newSpecies)
    {
        var plants = await greenhouseService.GetPlantsByGreenhouseIdAsync(greenhouseId);
        var existingPlant = plants.FirstOrDefault(p => p.Id == plantId);

        if (existingPlant == null)
            return NotFound($"Plant with ID {plantId} not found in greenhouse {greenhouseId}");

        existingPlant.UpdateSpecies(newSpecies);
        await greenhouseService.UpdatePlantInGreenhouseAsync(greenhouseId, plantId, existingPlant);
        return Ok("Plant species updated successfully.");
    }

    [HttpDelete("{greenhouseId}/plants/{plantId}")]
    public async Task<IActionResult> DeletePlantFromGreenhouse(int greenhouseId, int plantId)
    {
        await greenhouseService.DeletePlantFromGreenhouseAsync(greenhouseId, plantId);
        return Ok("Plant deleted from greenhouse successfully.");
    }

    // SENSOR ENDPOINTS
    [HttpGet("{id}/sensors")]
    public async Task<IActionResult> GetGreenhouseSensors(int id)
    {
        var sensors = await greenhouseService.GetSensorsByGreenhouseIdAsync(id);

        var simplifiedSensors = sensors.Select(s => new
        {
            s.Id,
            s.Type,
            s.Status
        }).ToList();

        return Ok(simplifiedSensors);
    }

    [HttpPost("{id}/sensors")]
    public async Task<IActionResult> AddSensorToGreenhouse(int id, [FromBody] SensorDto sensorDto)
    {
        var greenhouse = await greenhouseService.GetByIdAsync(id);
        var sensor = new Sensor(sensorDto.Type, sensorDto.Status, sensorDto.Unit, greenhouse);
        await greenhouseService.AddSensorToGreenhouseAsync(id, sensor);
        return Ok("Sensor added to greenhouse successfully.");
    }

    [HttpPut("{greenhouseId}/sensors/{sensorId}/status")]
    public async Task<IActionResult> UpdateSensorStatus(int greenhouseId, int sensorId, [FromBody] string newStatus)
    {
        var sensors = await greenhouseService.GetSensorsByGreenhouseIdAsync(greenhouseId);
        var existingSensor = sensors.FirstOrDefault(s => s.Id == sensorId);

        if (existingSensor == null)
            return NotFound($"Sensor with ID {sensorId} not found in greenhouse {greenhouseId}");

        existingSensor.UpdateStatus(newStatus);
        await greenhouseService.UpdateSensorInGreenhouseAsync(greenhouseId, sensorId, existingSensor);
        return Ok("Sensor status updated successfully.");
    }

    [HttpDelete("{greenhouseId}/sensors/{sensorId}")]
    public async Task<IActionResult> DeleteSensorFromGreenhouse(int greenhouseId, int sensorId)
    {
        await greenhouseService.DeleteSensorFromGreenhouseAsync(greenhouseId, sensorId);
        return Ok("Sensor deleted from greenhouse successfully.");
    }

    // ACTUATOR ENDPOINTS
    [HttpGet("{id}/actuators")]
    public async Task<IActionResult> GetGreenhouseActuators(int id)
    {
        var actuators = await greenhouseService.GetActuatorsByGreenhouseIdAsync(id);

        var simplifiedActuators = actuators.Select(a => new
        {
            a.Id,
            Type = a.GetType().Name.Replace("Actuator", ""),
            a.Status
        }).ToList();

        return Ok(simplifiedActuators);
    }

    [HttpPost("{id}/actuators")]
    public async Task<IActionResult> AddActuatorToGreenhouse(int id, [FromBody] ActuatorDto actuatorDto)
    {
        var greenhouse = await greenhouseService.GetByIdAsync(id);

        Actuator actuator = actuatorDto.Type.ToLower() switch
        {
            "waterPump" => new WaterPumpActuator(actuatorDto.Status, greenhouse),
            "servoMotor" => new ServoMotorActuator(actuatorDto.Status, greenhouse),
            _ => throw new ArgumentException($"Unsupported actuator type: {actuatorDto.Type}")
        };

        await greenhouseService.AddActuatorToGreenhouseAsync(id, actuator);
        return Ok("Actuator added to greenhouse successfully.");
    }

    [HttpPut("{greenhouseId}/actuators/{actuatorId}/status")]
    public async Task<IActionResult> UpdateActuatorStatus(int greenhouseId, int actuatorId, [FromBody] string newStatus)
    {
        var actuators = await greenhouseService.GetActuatorsByGreenhouseIdAsync(greenhouseId);
        var existingActuator = actuators.FirstOrDefault(a => a.Id == actuatorId);
        if (existingActuator == null)
            return NotFound($"Actuator with ID {actuatorId} not found in greenhouse {greenhouseId}");

        existingActuator.UpdateStatus(newStatus);
        await greenhouseService.UpdateActuatorInGreenhouseAsync(greenhouseId, actuatorId, existingActuator);
        return Ok("Actuator status updated successfully.");
    }

    [HttpDelete("{greenhouseId}/actuators/{actuatorId}")]
    public async Task<IActionResult> DeleteActuatorFromGreenhouse(int greenhouseId, int actuatorId)
    {
        await greenhouseService.DeleteActuatorFromGreenhouseAsync(greenhouseId, actuatorId);
        return Ok("Actuator deleted from greenhouse successfully.");
    }
}