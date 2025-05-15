using Domain.Entities;

namespace Domain.IRepositories;

public interface ISensorRepository : IBaseRepository<Sensor>
{
    Task<IEnumerable<Sensor>> GetByGreenhouseIdAsync(int greenhouseId);
}