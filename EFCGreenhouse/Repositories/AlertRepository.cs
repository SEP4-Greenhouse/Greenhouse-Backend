using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EFCGreenhouse.Repositories;

public class AlertRepository(GreenhouseDbContext context)
    : BaseRepository<Alert>(context), IAlertRepository
{
    public async Task<IEnumerable<Alert>> GetBySensorTypeAsync()
    {
        return await Context.Alerts
            .AsNoTracking()
            .Where(a => a.Type == Alert.AlertType.Sensor)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Alert>> GetByTypeAsync(Alert.AlertType type)
    {
        return await Context.Alerts
            .AsNoTracking()
            .Where(a => a.Type == type)
            .ToListAsync();
    }

    public async Task<IEnumerable<Alert>> GetByDateRangeAsync(DateTime start, DateTime end)
    {
        return await Context.Alerts
            .AsNoTracking()
            .Where(a => a.Timestamp >= start && a.Timestamp <= end)
            .ToListAsync();
    }

    public override async Task<Alert?> GetByIdAsync(int id)
    {
        return await Context.Alerts
            .Include(a => a.TriggeringSensorReadings)
            .Include(a => a.TriggeringActions)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}