using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EFCGreenhouse.Repositories;

public class SensorRepository : ISensorRepository
{
    private readonly GreenhouseDbContext _context;

    public SensorRepository(GreenhouseDbContext context)
    {
        _context = context;
    }

    public async Task<Sensor?> GetByIdAsync(int id)
    {
        return await _context.Set<Sensor>().FindAsync(id);
    }

    public async Task<IEnumerable<Sensor>> GetAllAsync()
    {
        return await _context.Set<Sensor>().ToListAsync();
    }

    public async Task AddAsync(Sensor sensor)
    {
        await _context.Set<Sensor>().AddAsync(sensor);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Sensor sensor)
    {
        _context.Set<Sensor>().Update(sensor);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var sensor = await GetByIdAsync(id);
        if (sensor != null)
        {
            _context.Set<Sensor>().Remove(sensor);
            await _context.SaveChangesAsync();
        }
    }
}