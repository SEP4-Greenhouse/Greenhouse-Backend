using Domain.Entities;

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
    Task RemovePlantFromGreenhouseAsync(int greenhouseId, int plantId);
    
    Task AddSensorToGreenhouseAsync(int greenhouseId, Sensor sensor);
    Task RemoveSensorFromGreenhouseAsync(int greenhouseId, int sensorId);
    
    Task AddActuatorToGreenhouseAsync(int greenhouseId, Actuator actuator);
    Task RemoveActuatorFromGreenhouseAsync(int greenhouseId, int actuatorId);
    
    // Specialized operations
    Task UpdatePlantTypeAsync(int greenhouseId, string newPlantType);
}