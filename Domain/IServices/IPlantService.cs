using Domain.Entities;

namespace Domain.IServices;

public interface IPlantService : IBaseService<Plant>
{
    Task<IEnumerable<Plant>> GetPlantsByGreenhouseIdAsync(int greenhouseId);
    Task<Plant> AddPlantToGreenhouseAsync(int greenhouseId, string species, DateTime plantingDate, string growthStage);
    Task UpdatePlantAsync(int plantId, string species, string growthStage);
}