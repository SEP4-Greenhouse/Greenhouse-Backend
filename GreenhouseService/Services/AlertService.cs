using Domain.Entities;
using Domain.IRepositories;
using Domain.IServices;
using GreenhouseService.Services;

namespace AlertService.Services
{
    public class AlertService : BaseService<Alert>, IAlertService
    {
        private readonly IAlertRepository _alertRepository;
        private readonly ISensorReadingRepository _sensorReadingRepository;
        private readonly IActuatorRepository _actuatorRepository;

        public AlertService(
            IAlertRepository alertRepository,
            ISensorReadingRepository sensorReadingRepository,
            IActuatorRepository actuatorRepository)
            : base(alertRepository)
        {
            _alertRepository = alertRepository;
            _sensorReadingRepository = sensorReadingRepository;
            _actuatorRepository = actuatorRepository;
        }

        // Query operations
        public async Task<IEnumerable<Alert>> GetAlertsBySensorTypeAsync()
        {
            return await _alertRepository.GetByTypeAsync(Alert.AlertType.Sensor);
        }

        public async Task<IEnumerable<Alert>> GetAlertsByTypeAsync(Alert.AlertType type)
        {
            return await _alertRepository.GetByTypeAsync(type);
        }

        public async Task<IEnumerable<Alert>> GetAlertsByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _alertRepository.GetByDateRangeAsync(start, end);
        }

        // Alert relationship management
        public async Task AddSensorReadingToAlertAsync(int alertId, SensorReading reading)
        {
            var alert = await _alertRepository.GetByIdAsync(alertId);
            if (alert == null)
                throw new KeyNotFoundException($"Alert with ID {alertId} not found");

            alert.AddTriggeringSensorReading(reading);
            await _alertRepository.UpdateAsync(alert);
        }

        public async Task AddActuatorActionToAlertAsync(int alertId, ActuatorAction action)
        {
            var alert = await _alertRepository.GetByIdAsync(alertId);
            if (alert == null)
                throw new KeyNotFoundException($"Alert with ID {alertId} not found");

            alert.AddTriggeringAction(action);
            await _alertRepository.UpdateAsync(alert);
        }

        // Alert generation
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

        // Helper method for alert creation (moved from interface)
        public async Task<Alert> CreateAlertAsync(Alert.AlertType type, string message)
        {
            var alert = new Alert(type, message);
            return await AddAsync(alert);
        }

        // Helper method for alert updates (moved from interface)
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