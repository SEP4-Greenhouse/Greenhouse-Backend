using Domain.Entities;

namespace Domain.IRepositories;

public interface IAlertRepository : IBaseRepository<Alert>
{
    Task<IEnumerable<Alert>> GetBySensorTypeAsync();
    Task<IEnumerable<Alert>> GetByTypeAsync(Alert.AlertType type);
    Task<IEnumerable<Alert>> GetByDateRangeAsync(DateTime start, DateTime end);
}