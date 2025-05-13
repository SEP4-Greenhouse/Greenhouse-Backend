using Domain.Entities;

namespace Domain.IRepositories;

public interface IAlertRepository : IBaseRepository<Alert>
{
    Task<IEnumerable<Alert>> GetBySensorTypeAsync();
}