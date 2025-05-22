using Domain.Entities;
using Domain.IRepositories;
using Domain.IServices;

namespace GreenhouseService.Services
{
    public class AlertService(IAlertRepository alertRepository) : BaseService<Alert>(alertRepository), IAlertService
    {
        public async Task<IEnumerable<Alert>> GetAlertsByTypeAsync(Alert.AlertType type)
        {
            return await alertRepository.GetByTypeAsync(type);
        }

        public async Task<IEnumerable<Alert>> GetAlertsByDateRangeAsync(DateTime start, DateTime end)
        {
            return await alertRepository.GetByDateRangeAsync(start, end);
        }

        public async Task<IEnumerable<Alert>> GetAllAlertsAsync()
        {
            return await alertRepository.GetAllAsync();
        }

        public async Task CreateSensorAlertAsync(SensorReading reading, string message)
        {
            var alert = new Alert(Alert.AlertType.Sensor, message);
            alert.AddTriggeringSensorReading(reading);
            await alertRepository.AddAsync(alert);
        }

        public async Task CreateActuatorAlertAsync(ActuatorAction action, string message)
        {
            var alert = new Alert(Alert.AlertType.Actuator, message);
            alert.AddTriggeringActuatorAction(action);
            await alertRepository.AddAsync(alert);
        }
    }
}