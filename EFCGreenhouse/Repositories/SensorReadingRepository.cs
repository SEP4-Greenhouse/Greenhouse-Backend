using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCGreenhouse.Repositories;

public class SensorReadingRepository : ISensorReadingRepository
{
    private readonly GreenhouseDbContext _context;
    private readonly ILogger<SensorReadingRepository> _logger;

    public SensorReadingRepository(GreenhouseDbContext context, ILogger<SensorReadingRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<SensorReading?> GetByIdAsync(int id)
    {
        return await _context.SensorReadings.FindAsync(id);
    }

    public async Task<IEnumerable<SensorReading>> GetAllAsync()
    {
        return await _context.SensorReadings
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<SensorReading>> GetLatestFromAllSensorsAsync()
    {
        try
        {
            // More efficient query using window functions
            return await _context.SensorReadings
                .FromSqlRaw(@"
                    SELECT sr.*
                    FROM (
                        SELECT *, ROW_NUMBER() OVER(PARTITION BY ""SensorId"" ORDER BY ""TimeStamp"" DESC) as rn
                        FROM ""SensorReadings""
                    ) sr
                    WHERE sr.rn = 1")
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting latest readings from all sensors");
            throw;
        }
    }

    public async Task<IDictionary<int, IEnumerable<SensorReading>>> GetAllGroupedBySensorAsync()
    {
        try
        {
            var readings = await _context.SensorReadings
                .AsNoTracking()
                .OrderByDescending(r => r.TimeStamp)
                .ToListAsync();

            return readings
                .GroupBy(r => r.SensorId)
                .ToDictionary(g => g.Key, g => (IEnumerable<SensorReading>)g.ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error grouping readings by sensor");
            throw;
        }
    }

    public async Task<IEnumerable<SensorReading>> GetByTimeRangeAsync(DateTime start, DateTime end)
    {
        return await _context.SensorReadings
            .AsNoTracking()
            .Where(r => r.TimeStamp >= start && r.TimeStamp <= end)
            .OrderByDescending(r => r.TimeStamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<SensorReading>> GetPaginatedAsync(int sensorId, int pageNumber, int pageSize)
    {
        return await _context.SensorReadings
            .AsNoTracking()
            .Where(r => r.SensorId == sensorId)
            .OrderByDescending(r => r.TimeStamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<double> GetAverageAsync(int sensorId, DateTime start, DateTime end)
    {
        return await _context.SensorReadings
            .Where(r => r.SensorId == sensorId && r.TimeStamp >= start && r.TimeStamp <= end)
            .Select(r => r.Value)
            .AverageAsync();
    }

    public async Task<IDictionary<int, SensorReading>> GetLatestBySensorAsync()
    {
        try
        {
            // Use SQL for better performance with window functions
            var latestReadings = await _context.SensorReadings
                .FromSqlRaw(@"
                    SELECT sr.*
                    FROM (
                        SELECT *, ROW_NUMBER() OVER(PARTITION BY ""SensorId"" ORDER BY ""TimeStamp"" DESC) as rn
                        FROM ""SensorReadings""
                    ) sr
                    WHERE sr.rn = 1")
                .AsNoTracking()
                .ToListAsync();

            return latestReadings.ToDictionary(r => r.SensorId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting latest readings by sensor");
            throw;
        }
    }

    public async Task AddAsync(SensorReading sensorReading)
    {
        try
        {
            await _context.SensorReadings.AddAsync(sensorReading);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding sensor reading");
            throw;
        }
    }

    public async Task UpdateAsync(SensorReading sensorReading)
    {
        try
        {
            _context.Entry(sensorReading).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating sensor reading {Id}", sensorReading.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        var sensorReading = await GetByIdAsync(id);
        if (sensorReading != null)
        {
            try
            {
                _context.SensorReadings.Remove(sensorReading);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting sensor reading {Id}", id);
                throw;
            }
        }
    }
}