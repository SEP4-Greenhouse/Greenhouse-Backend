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

        public async Task<SensorReading?> GetReadingByIdAsync(int id)
        {
            return await _context.SensorReadings
                .Include(r => r.Sensor)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<SensorReading>> GetReadingsBySensorIdAsync(int sensorId)
        {
            return await _context.SensorReadings
                .Where(r => r.SensorId == sensorId)
                .OrderByDescending(r => r.TimeStamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<SensorReading>> GetLatestReadingsAsync(int count)
        {
            return await _context.SensorReadings
                .OrderByDescending(r => r.TimeStamp)
                .Take(count)
                .Include(r => r.Sensor)
                .ToListAsync();
        }

        public async Task AddSensorReadingAsync(SensorReading reading)
        {
            await _context.SensorReadings.AddAsync(reading);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SensorReading>> GetReadingsInRangeAsync(int sensorId, DateTime start, DateTime end)
        {
            return await _context.SensorReadings
                .Where(r => r.SensorId == sensorId && r.TimeStamp >= start && r.TimeStamp <= end)
                .OrderBy(r => r.TimeStamp)
                .ToListAsync();
        }

        public async Task TriggerAlertIfNecessaryAsync(SensorReading reading)
        {
            // Adjust thresholds as per your requirements (not implemented yet)
            double tempThreshold = 35.0; 
            double humidityThreshold = 20.0; 

            bool shouldTrigger = false;
            string message = "";
            string type = "";

            if (reading.Sensor.Type.ToLower().Contains("temperature") && reading.Value > tempThreshold)
            {
                shouldTrigger = true;
                message = $"High temperature detected: {reading.Value} {reading.Unit}";
                type = "Temperature";
            }
            else if (reading.Sensor.Type.ToLower().Contains("humidity") && reading.Value < humidityThreshold)
            {
                shouldTrigger = true;
                message = $"Low humidity detected: {reading.Value} {reading.Unit}";
                type = "Humidity";
            }

            if (shouldTrigger)
            {
                var alert = reading.TriggerAlert(type, message);
                await _context.Alerts.AddAsync(alert);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<SensorReading>> GetLatestReadingsForAllSensorsAsync()
        {
            return await _context.SensorReadings
                .GroupBy(r => r.SensorId)
                .Select(group => group.OrderByDescending(r => r.TimeStamp).First())
                .Include(r => r.Sensor)
                .ToListAsync();
        }

        public async Task<IEnumerable<SensorReading>> GetReadingsBySensorTypeAsync(string sensorType)
        {
            return await _context.SensorReadings
                .Where(r => r.Sensor.Type.Equals(sensorType, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(r => r.TimeStamp)
                .ToListAsync();
        }
    }
}
