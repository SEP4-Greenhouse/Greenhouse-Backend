using Domain.Entities;
using Domain.IServices;
using EFCGreenhouse;
using Microsoft.EntityFrameworkCore;

namespace GreenhouseService.Services
{
    public class SensorDataService : ISensorDataService
    {
        private readonly GreenhouseDbContext _context;

        public SensorDataService(GreenhouseDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SensorReading>> GetLatestReadingFromAllSensorsAsync()
        {
            return await _context.SensorReadings
                .GroupBy(r => r.SensorId)
                .Select(g => g.OrderByDescending(r => r.TimeStamp).First())
                .Include(r => r.Sensor)
                .ToListAsync();
        }

        public async Task<IDictionary<int, IEnumerable<SensorReading>>> GetReadingsBySensorAsync()
        {
            return await _context.SensorReadings
                .Include(r => r.Sensor)
                .GroupBy(r => r.SensorId)
                .ToDictionaryAsync(g => g.Key, g => g.AsEnumerable());
        }

        public async Task<IEnumerable<SensorReading>> GetReadingsByTimestampRangeAsync(DateTime start, DateTime end)
        {
            return await _context.SensorReadings
                .Where(r => r.TimeStamp >= start && r.TimeStamp <= end)
                .Include(r => r.Sensor)
                .ToListAsync();
        }

        public async Task<IEnumerable<SensorReading>> GetReadingsPaginatedAsync(int sensorId, int pageNumber,
            int pageSize)
        {
            return await _context.SensorReadings
                .Where(r => r.SensorId == sensorId)
                .OrderByDescending(r => r.TimeStamp)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<double> GetAverageReadingForSensorAsync(int sensorId, DateTime start, DateTime end)
        {
            return await _context.SensorReadings
                .Where(r => r.SensorId == sensorId && r.TimeStamp >= start && r.TimeStamp <= end)
                .AverageAsync(r => r.Value);
        }

        public async Task<IDictionary<int, SensorReading>> GetLatestReadingBySensorAsync()
        {
            return await _context.SensorReadings
                .GroupBy(r => r.SensorId)
                .Select(g => g.OrderByDescending(r => r.TimeStamp).First())
                .ToDictionaryAsync(r => r.SensorId);
        }

        public async Task AddSensorReadingAsync(SensorReading reading)
        {
            await _context.SensorReadings.AddAsync(reading);
            await _context.SaveChangesAsync();
            await TriggerAlertIfThresholdExceededAsync(reading);
        }

        public async Task TriggerAlertIfThresholdExceededAsync(SensorReading reading)
        {
            //thresholds for temperature and humidity (example values)
            // TODO: Add thresholds class for each sensor type for setting thresholds from the UI
            double tempThreshold = 35.0;
            double humidityThreshold = 20.0;

            Alert? alert = null;

            if (reading.Sensor.Type.ToLower().Contains("temperature") && reading.Value > tempThreshold)
            {
                alert = new Alert(Alert.AlertType.Sensor, $"High temperature detected: {reading.Value} {reading.Unit}");
            }
            else if (reading.Sensor.Type.ToLower().Contains("humidity") && reading.Value < humidityThreshold)
            {
                alert = new Alert(Alert.AlertType.Sensor, $"Low humidity detected: {reading.Value} {reading.Unit}");
            }

            if (alert != null)
            {
                await _context.Alerts.AddAsync(alert);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Alert>> GetAllSensorsReadingAlertsAsync()
        {
            return await _context.Alerts
                .Where(a => a.Type == Alert.AlertType.Sensor)
                .ToListAsync();
        }

        public async Task AddSensorAsync(Sensor sensor)
        {
            if (sensor == null)
                throw new ArgumentNullException(nameof(sensor));
            if (await _context.Sensors.AnyAsync(s => s.Id == sensor.Id))
                throw new InvalidOperationException($"A sensor with ID {sensor.Id} already exists.");

            await _context.Sensors.AddAsync(sensor);
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteSensorAsync(int sensorId)
        {
            if (sensorId <= 0)
                throw new ArgumentException("Sensor ID must be greater than zero.");

            var sensor = await _context.Sensors.Include(s => s.Readings).FirstOrDefaultAsync(s => s.Id == sensorId);
            if (sensor == null)
                throw new KeyNotFoundException("Sensor not found.");

            _context.Sensors.Remove(sensor);
            await _context.SaveChangesAsync();
        }
    }
}