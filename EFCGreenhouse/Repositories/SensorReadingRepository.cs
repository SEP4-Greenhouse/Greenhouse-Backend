using Domain.Entities;
using Domain.IRepositories;
using EFCGreenhouse.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCGreenhouse.Repositories;

public class SensorReadingRepository : BaseRepository<SensorReading>, ISensorReadingRepository
{
    private readonly ILogger<SensorReadingRepository> _logger;

    public SensorReadingRepository(GreenhouseDbContext context, ILogger<SensorReadingRepository> logger) 
        : base(context)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<SensorReading>> GetLatestFromAllSensorsAsync()
    {
        try
        {
            return await Context.SensorReadings
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
            var readings = await DbSet
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
        return await DbSet
            .AsNoTracking()
            .Where(r => r.TimeStamp >= start && r.TimeStamp <= end)
            .OrderByDescending(r => r.TimeStamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<SensorReading>> GetPaginatedAsync(int sensorId, int pageNumber, int pageSize)
    {
        return await DbSet
            .AsNoTracking()
            .Where(r => r.SensorId == sensorId)
            .OrderByDescending(r => r.TimeStamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<double> GetAverageAsync(int sensorId, DateTime start, DateTime end)
    {
        return await DbSet
            .Where(r => r.SensorId == sensorId && r.TimeStamp >= start && r.TimeStamp <= end)
            .Select(r => r.Value)
            .AverageAsync();
    }

    public async Task<IDictionary<int, SensorReading>> GetLatestBySensorAsync()
    {
        try
        {
            var latestReadings = await Context.SensorReadings
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
}