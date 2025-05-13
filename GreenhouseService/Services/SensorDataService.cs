using Domain.Entities;
using Domain.IRepositories;
using Domain.IServices;

namespace GreenhouseService.Services
{
    public class SensorDataService : ISensorDataService
    {
        private readonly ISensorRepository _sensorRepository;
        private readonly ISensorReadingRepository _sensorReadingRepository;
        private readonly IAlertRepository _alertRepository;

        public SensorDataService(
            ISensorRepository sensorRepository,
            ISensorReadingRepository sensorReadingRepository,
            IAlertRepository alertRepository)
        {
            _sensorRepository = sensorRepository;
            _sensorReadingRepository = sensorReadingRepository;
            _alertRepository = alertRepository;
        }

        public async Task<IEnumerable<SensorReading>> GetLatestReadingFromAllSensorsAsync()
        {
            return await _sensorReadingRepository.GetLatestFromAllSensorsAsync();
        }

        public async Task<IDictionary<int, IEnumerable<SensorReading>>> GetReadingsBySensorAsync()
        {
            return await _sensorReadingRepository.GetAllGroupedBySensorAsync();
        }

        public async Task<IEnumerable<SensorReading>> GetReadingsByTimestampRangeAsync(DateTime start, DateTime end)
        {
            return await _sensorReadingRepository.GetByTimeRangeAsync(start, end);
        }

        public async Task<IEnumerable<SensorReading>> GetReadingsPaginatedAsync(int sensorId, int pageNumber, int pageSize)
        {
            return await _sensorReadingRepository.GetPaginatedAsync(sensorId, pageNumber, pageSize);
        }

        public async Task<double> GetAverageReadingForSensorAsync(int sensorId, DateTime start, DateTime end)
        {
            return await _sensorReadingRepository.GetAverageAsync(sensorId, start, end);
        }

        public async Task<IDictionary<int, SensorReading>> GetLatestReadingBySensorAsync()
        {
            return await _sensorReadingRepository.GetLatestBySensorAsync();
        }

        public async Task AddSensorReadingAsync(SensorReading reading)
        {
            await _sensorReadingRepository.AddAsync(reading);
            await TriggerAlertIfThresholdExceededAsync(reading);
        }

        public async Task TriggerAlertIfThresholdExceededAsync(SensorReading reading)
        {
            double tempThreshold = 35.0;
            double humidityThreshold = 20.0;

            Alert? alert = null;

            if (reading.Sensor.Type.ToLower().Contains("temperature") && reading.Value > tempThreshold)
            {
                alert = new Alert(Alert.AlertType.Sensor, $"High temperature detected: {reading.Value} {reading.Unit}");
                alert.AddTriggeringSensorReading(reading);
            }
            else if (reading.Sensor.Type.ToLower().Contains("humidity") && reading.Value < humidityThreshold)
            {
                alert = new Alert(Alert.AlertType.Sensor, $"Low humidity detected: {reading.Value} {reading.Unit}");
                alert.AddTriggeringSensorReading(reading);
            }

            if (alert != null)
            {
                await _alertRepository.AddAsync(alert);
            }
        }

        public async Task<IEnumerable<Alert>> GetAllSensorsReadingAlertsAsync()
        {
            return await _alertRepository.GetBySensorTypeAsync();
        }

        public async Task AddSensorAsync(Sensor sensor)
        {
            if (sensor == null)
                throw new ArgumentNullException(nameof(sensor));
            if (await _sensorRepository.ExistsByIdAsync(sensor.Id))
                throw new InvalidOperationException($"A sensor with ID {sensor.Id} already exists.");

            await _sensorRepository.AddAsync(sensor);
        }
        
        public async Task DeleteSensorAsync(int sensorId)
        {
            await _sensorRepository.DeleteAsync(sensorId);
        }
    }
}