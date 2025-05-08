using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EFCGreenhouse.Repositories;

public class SensorReadingRepository : ISensorReadingRepository
{
    private readonly GreenhouseDbContext _context;

    public SensorReadingRepository(GreenhouseDbContext context)
    {
        _context = context;
    }

    public async Task<SensorReading?> GetByIdAsync(int id)
    {
        return await _context.Set<SensorReading>().FindAsync(id);
    }

    public async Task<IEnumerable<SensorReading>> GetAllAsync()
    {
        return await _context.Set<SensorReading>().ToListAsync();
    }

    public async Task AddAsync(SensorReading sensorReading)
    {
        await _context.Set<SensorReading>().AddAsync(sensorReading);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(SensorReading sensorReading)
    {
        _context.Set<SensorReading>().Update(sensorReading);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var sensorReading = await GetByIdAsync(id);
        if (sensorReading != null)
        {
            _context.Set<SensorReading>().Remove(sensorReading);
            await _context.SaveChangesAsync();
        }
    }
}