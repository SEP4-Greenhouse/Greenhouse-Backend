using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EFCGreenhouse.Repositories;

public class ThresholdRepository(GreenhouseDbContext context) : IThresholdRepository
{
    public async Task<Threshold> GetThresholdBySensorIdAsync(int sensorId) =>
        await context.Set<Threshold>()
            .Include(t => t.Sensor)
            .FirstOrDefaultAsync(t => t.SensorId == sensorId) ?? throw new InvalidOperationException();

    public async Task<IEnumerable<Threshold>> GetAllThresholdsAsync()
    {
        return await context.Set<Threshold>()
            .Include(t => t.Sensor)
            .ToListAsync();
    }

    public async Task<Threshold> AddThresholdAsync(Threshold threshold)
    {
        context.Set<Threshold>().Add(threshold);
        await context.SaveChangesAsync();
        return threshold;
    }

    public async Task<Threshold> UpdateThresholdAsync(Threshold threshold)
    {
        context.Set<Threshold>().Update(threshold);
        await context.SaveChangesAsync();
        return threshold;
    }

    public async Task<bool> DeleteThresholdAsync(int id)
    {
        var threshold = await context.Set<Threshold>().FindAsync(id);
        if (threshold == null) return false;
        context.Set<Threshold>().Remove(threshold);
        await context.SaveChangesAsync();
        return true;
    }
}