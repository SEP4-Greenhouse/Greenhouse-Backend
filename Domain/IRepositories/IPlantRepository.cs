using Domain.Entities;

namespace Domain.IRepositories;

public interface IPlantRepository : IBaseRepository<Plant>
{
    Task<IEnumerable<Plant>> GetByGreenhouseIdAsync(int greenhouseId);
}