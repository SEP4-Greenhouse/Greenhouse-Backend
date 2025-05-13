using Domain.Entities;
using Domain.IRepositories;
using Domain.IServices;

namespace GreenhouseService.Services
{
    public class SensorDataService(
        ISensorRepository sensorRepository,
        ISensorReadingRepository sensorReadingRepository,
        IAlertRepository alertRepository)
        : ISensorDataService
    {
        public async Task<IEnumerable<SensorReading>> GetLatestReadingFromAllSensorsAsync()
        {
            return await sensorReadingRepository.GetLatestFromAllSensorsAsync();
        }

        public async Task<IDictionary<int, IEnumerable<SensorReading>>> GetReadingsBySensorAsync()
        {
            return await sensorReadingRepository.GetAllGroupedBySensorAsync();
        }

        public async Task<IEnumerable<SensorReading>> GetReadingsByTimestampRangeAsync(DateTime start, DateTime end)
        {
            return await sensorReadingRepository.GetByTimeRangeAsync(start, end);
        }

        public async Task<IEnumerable<SensorReading>> GetReadingsPaginatedAsync(int sensorId, int pageNumber,
            int pageSize)
        {
            return await sensorReadingRepository.GetPaginatedAsync(sensorId, pageNumber, pageSize);
        }

        public async Task<double> GetAverageReadingForSensorAsync(int sensorId, DateTime start, DateTime end)
        {
            return await sensorReadingRepository.GetAverageAsync(sensorId, start, end);
        }

        public async Task<IDictionary<int, SensorReading>> GetLatestReadingBySensorAsync()
        {
            return await sensorReadingRepository.GetLatestBySensorAsync();
        }

        public async Task AddSensorReadingAsync(SensorReading reading)
        {
            await sensorReadingRepository.AddAsync(reading);
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
                await alertRepository.AddAsync(alert);
            }
        }

        public async Task<IEnumerable<Alert>> GetAllSensorsReadingAlertsAsync()
        {
            return await alertRepository.GetBySensorTypeAsync();
        }

        public async Task AddSensorAsync(Sensor sensor)
        {
            if (sensor == null)
                throw new ArgumentNullException(nameof(sensor));
            if (await sensorRepository.ExistsByIdAsync(sensor.Id))
                throw new InvalidOperationException($"A sensor with ID {sensor.Id} already exists.");

            await sensorRepository.AddAsync(sensor);
        }

        public async Task DeleteSensorAsync(int sensorId)
        {
            await sensorRepository.DeleteAsync(sensorId);
        }
    }
}