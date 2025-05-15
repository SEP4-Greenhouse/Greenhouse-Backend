using Domain.Entities;

namespace Domain.IRepositories;

public interface IGreenhouseRepository : IBaseRepository<Greenhouse>
{
    Task<IEnumerable<Greenhouse>> GetByUserIdAsync(int userId);
}