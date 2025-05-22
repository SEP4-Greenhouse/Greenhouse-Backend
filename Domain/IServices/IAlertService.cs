using Domain.Entities;

namespace Domain.IServices
{
    public interface IAlertService : IBaseService<Alert>
    {
        Task<IEnumerable<Alert>> GetAlertsBySensorTypeAsync();
        Task<IEnumerable<Alert>> GetAlertsByTypeAsync(Alert.AlertType type);
        Task<IEnumerable<Alert>> GetAlertsByDateRangeAsync(DateTime start, DateTime end);

        Task AddSensorReadingToAlertAsync(int alertId, SensorReading reading);
        Task AddActuatorActionToAlertAsync(int alertId, ActuatorAction action);

        Task<Alert> CreateSensorAlertAsync(SensorReading reading, string message);
        Task<Alert> CreateActuatorAlertAsync(ActuatorAction action, string message);
        Task<Alert> CreateSystemAlertAsync(string message);
    }
}