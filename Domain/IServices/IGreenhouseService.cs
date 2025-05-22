using Domain.Entities;
using Domain.Entities.Actuators;

namespace Domain.IServices;

public interface IGreenhouseService : IBaseService<Greenhouse>
{
    // Relationship queries 
    Task<IEnumerable<Greenhouse>> GetGreenhousesByUserIdAsync(int userId);
    Task<IEnumerable<Sensor>> GetSensorsByGreenhouseIdAsync(int greenhouseId);
    Task<IEnumerable<Actuator>> GetActuatorsByGreenhouseIdAsync(int greenhouseId);
    Task<IEnumerable<Plant>> GetPlantsByGreenhouseIdAsync(int greenhouseId);
    
    // Relationship management
    Task<Plant> AddPlantToGreenhouseAsync(int greenhouseId, Plant plant);
    Task DeletePlantFromGreenhouseAsync(int greenhouseId, int plantId);
    
    Task AddSensorToGreenhouseAsync(int greenhouseId, Sensor sensor);
    Task DeleteSensorFromGreenhouseAsync(int greenhouseId, int sensorId);
    
    Task AddActuatorToGreenhouseAsync(int greenhouseId, Actuator actuator);
    Task DeleteActuatorFromGreenhouseAsync(int greenhouseId, int actuatorId);
    Task UpdateSensorInGreenhouseAsync(int greenhouseId, int sensorId, Sensor updatedSensor);
    Task UpdateActuatorInGreenhouseAsync(int greenhouseId, int actuatorId, Actuator updatedActuator);
    Task UpdatePlantInGreenhouseAsync(int greenhouseId, int plantId, Plant updatedPlant);
}