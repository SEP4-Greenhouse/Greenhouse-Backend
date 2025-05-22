using Domain.Entities;

namespace Domain.IServices
{
    public interface IAlertService : IBaseService<Alert>
    {
        Task<IEnumerable<Alert>> GetAlertsByTypeAsync(Alert.AlertType type);
        Task<IEnumerable<Alert>> GetAlertsByDateRangeAsync(DateTime start, DateTime end);
        Task<IEnumerable<Alert>> GetAllAlertsAsync();
        Task CreateSensorAlertAsync(SensorReading reading, string message);
        Task CreateActuatorAlertAsync(ActuatorAction action, string message);
    }
}