using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EFCGreenhouse.Repositories;

public class ThresholdRepository : IThresholdRepository
{
    private readonly GreenhouseDbContext _context;

    public ThresholdRepository(GreenhouseDbContext context)
    {
        _context = context;
    }

    public async Task<Threshold> GetThresholdBySensorIdAsync(int sensorId)
    {
        return await _context.Set<Threshold>()
            .Include(t => t.Sensor)
            .FirstOrDefaultAsync(t => t.SensorId == sensorId);
    }

    public async Task<IEnumerable<Threshold>> GetAllThresholdsAsync()
    {
        return await _context.Set<Threshold>()
            .Include(t => t.Sensor)
            .ToListAsync();
    }

    public async Task<Threshold> AddThresholdAsync(Threshold threshold)
    {
        _context.Set<Threshold>().Add(threshold);
        await _context.SaveChangesAsync();
        return threshold;
    }

    public async Task<Threshold> UpdateThresholdAsync(Threshold threshold)
    {
        _context.Set<Threshold>().Update(threshold);
        await _context.SaveChangesAsync();
        return threshold;
    }

    public async Task<bool> DeleteThresholdAsync(int id)
    {
        var threshold = await _context.Set<Threshold>().FindAsync(id);
        if (threshold == null) return false;
        _context.Set<Threshold>().Remove(threshold);
        await _context.SaveChangesAsync();
        return true;
    }
}