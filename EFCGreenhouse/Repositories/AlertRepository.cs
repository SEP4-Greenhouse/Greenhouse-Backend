using Domain.Entities;
using Domain.IRepositories;
using EFCGreenhouse;
using EFCGreenhouse.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class AlertRepository : BaseRepository<Alert>, IAlertRepository
{
    private readonly ILogger<AlertRepository> _logger;

    public AlertRepository(GreenhouseDbContext context, ILogger<AlertRepository> logger) 
        : base(context)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<Alert>> GetBySensorTypeAsync()
    {
        return await Context.Alerts
            .AsNoTracking()
            .Where(a => a.Type == Alert.AlertType.Sensor)
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