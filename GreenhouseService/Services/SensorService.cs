using Domain.Entities;
using Domain.IRepositories;
using Domain.IServices;

namespace GreenhouseService.Services
{
    public class SensorService(
        ISensorRepository sensorRepository,
        ISensorReadingRepository sensorReadingRepository,
        IAlertService alertService)
        : BaseService<Sensor>(sensorRepository), ISensorService
    {
        public async Task<SensorReading> GetReadingByIdAsync(int readingId)
        {
            return await sensorReadingRepository.GetByIdAsync(readingId);
        }

        public async Task AddSensorReadingAsync(SensorReading reading)
        {
            await sensorReadingRepository.AddAsync(reading);

            await CheckReadingThresholdsAsync(reading);
        }

        public async Task UpdateSensorReadingAsync(SensorReading reading)
        {
            await sensorReadingRepository.UpdateAsync(reading);
            await CheckReadingThresholdsAsync(reading);
        }

        public async Task DeleteSensorReadingAsync(int readingId)
        {
            await sensorReadingRepository.DeleteAsync(readingId);
        }

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

        private async Task CheckReadingThresholdsAsync(SensorReading reading)
        {
            double tempThreshold = 35.0;
            double humidityThreshold = 20.0;

            if (reading.Sensor.Type.ToLower().Contains("temperature") && reading.Value > tempThreshold)
            {
                await alertService.CreateSensorAlertAsync(reading,
                    $"High temperature detected: {reading.Value} {reading.Unit}");
            }
            else if (reading.Sensor.Type.ToLower().Contains("humidity") && reading.Value < humidityThreshold)
            {
                await alertService.CreateSensorAlertAsync(reading,
                    $"Low humidity detected: {reading.Value} {reading.Unit}");
            }
        }
    }
}