using Domain.Entities;

namespace Domain.IServices
{
    public interface IAlertService : IBaseService<Alert>
    {
        // Query operations
        Task<IEnumerable<Alert>> GetAlertsBySensorTypeAsync();
        Task<IEnumerable<Alert>> GetAlertsByTypeAsync(Alert.AlertType type);
        Task<IEnumerable<Alert>> GetAlertsByDateRangeAsync(DateTime start, DateTime end);

        // Alert relationship management
        Task AddSensorReadingToAlertAsync(int alertId, SensorReading reading);
        Task AddActuatorActionToAlertAsync(int alertId, ActuatorAction action);

        // Alert generation
        Task<Alert> CreateSensorAlertAsync(SensorReading reading, string message);
        Task<Alert> CreateActuatorAlertAsync(ActuatorAction action, string message);
        Task<Alert> CreateSystemAlertAsync(string message);
    }
}