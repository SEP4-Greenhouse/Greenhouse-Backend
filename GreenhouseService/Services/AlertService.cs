using Domain.Entities;
using Domain.IRepositories;
using Domain.IServices;

namespace GreenhouseService.Services
{
    public class AlertService(IAlertRepository alertRepository) : BaseService<Alert>(alertRepository), IAlertService
    {
        public async Task<IEnumerable<Alert>> GetAlertsBySensorTypeAsync()
        {
            return await alertRepository.GetByTypeAsync(Alert.AlertType.Sensor);
        }

        public async Task<IEnumerable<Alert>> GetAlertsByTypeAsync(Alert.AlertType type)
        {
            return await alertRepository.GetByTypeAsync(type);
        }

        public async Task<IEnumerable<Alert>> GetAlertsByDateRangeAsync(DateTime start, DateTime end)
        {
            return await alertRepository.GetByDateRangeAsync(start, end);
        }

        public async Task AddSensorReadingToAlertAsync(int alertId, SensorReading reading)
        {
            var alert = await alertRepository.GetByIdAsync(alertId);
            if (alert == null)
                throw new KeyNotFoundException($"Alert with ID {alertId} not found");

            alert.AddTriggeringSensorReading(reading);
            await alertRepository.UpdateAsync(alert);
        }

        public async Task AddActuatorActionToAlertAsync(int alertId, ActuatorAction action)
        {
            var alert = await alertRepository.GetByIdAsync(alertId);
            if (alert == null)
                throw new KeyNotFoundException($"Alert with ID {alertId} not found");

            alert.AddTriggeringAction(action);
            await alertRepository.UpdateAsync(alert);
        }

        public async Task<Alert> CreateSensorAlertAsync(SensorReading reading, string message)
        {
            if (reading == null)
                throw new ArgumentNullException(nameof(reading));

            var alert = new Alert(Alert.AlertType.Sensor, message);
            alert.AddTriggeringSensorReading(reading);

            return await AddAsync(alert);
        }

        public async Task<Alert> CreateActuatorAlertAsync(ActuatorAction action, string message)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var alert = new Alert(Alert.AlertType.Controller, message);
            alert.AddTriggeringAction(action);

            return await AddAsync(alert);
        }

        public async Task<Alert> CreateSystemAlertAsync(string message)
        {
            var alert = new Alert(Alert.AlertType.System, message);
            return await AddAsync(alert);
        }

        public async Task<Alert> CreateAlertAsync(Alert.AlertType type, string message)
        {
            var alert = new Alert(type, message);
            return await AddAsync(alert);
        }

        public async Task UpdateAlertAsync(int id, string message)
        {
            var alert = await GetByIdAsync(id);
            if (alert == null)
                throw new KeyNotFoundException($"Alert with ID {id} not found");

            alert.UpdateMessage(message);
            await UpdateAsync(alert);
        }
    }
}