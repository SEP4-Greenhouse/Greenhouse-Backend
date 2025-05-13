using Domain.Entities;
using Domain.IRepositories;
using EFCGreenhouse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class AlertRepository : IAlertRepository
{
    private readonly GreenhouseDbContext _context;
    private readonly ILogger<AlertRepository> _logger;

    public AlertRepository(GreenhouseDbContext context, ILogger<AlertRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Alert?> GetByIdAsync(int id)
    {
        return await _context.Alerts
            .Include(a => a.TriggeringSensorReadings)
            .Include(a => a.TriggeringActions)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Alert>> GetAllAsync()
    {
        return await _context.Alerts
            .AsNoTracking()
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Alert>> GetBySensorTypeAsync()
    {
        return await _context.Alerts
            .AsNoTracking()
            .Where(a => a.Type == Alert.AlertType.Sensor)
            .ToListAsync();
    }

    public async Task AddAsync(Alert alert)
    {
        try
        {
            await _context.Alerts.AddAsync(alert);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding alert");
            throw;
        }
    }

    public async Task UpdateAsync(Alert alert)
    {
        try
        {
            _context.Entry(alert).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating alert {Id}", alert.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        var alert = await GetByIdAsync(id);
        if (alert != null)
        {
            try
            {
                _context.Alerts.Remove(alert);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting alert {Id}", id);
                throw;
            }
        }
    }
}