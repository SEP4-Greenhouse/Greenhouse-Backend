using Domain.Entities;
using Domain.IRepositories;
using EFCGreenhouse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class PredictionLogRepository : IPredictionLogRepository
{
    private readonly GreenhouseDbContext _context;
    private readonly ILogger<PredictionLogRepository> _logger;

    public PredictionLogRepository(GreenhouseDbContext context, ILogger<PredictionLogRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddAsync(PredictionLog log)
    {
        try
        {
            await _context.PredictionLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding prediction log");
            throw;
        }
    }

    public async Task<IEnumerable<PredictionLog>> GetAllAsync()
    {
        return await _context.PredictionLogs
            .AsNoTracking()
            .OrderByDescending(p => p.Timestamp)
            .ToListAsync();
    }

    public async Task<PredictionLog?> GetByIdAsync(int id)
    {
        return await _context.PredictionLogs.FindAsync(id);
    }

    public async Task UpdateAsync(PredictionLog log)
    {
        try
        {
            _context.Entry(log).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating prediction log {Id}", log.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        var log = await _context.PredictionLogs.FindAsync(id);
        if (log != null)
        {
            try
            {
                _context.PredictionLogs.Remove(log);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting prediction log {Id}", id);
                throw;
            }
        }
    }
}