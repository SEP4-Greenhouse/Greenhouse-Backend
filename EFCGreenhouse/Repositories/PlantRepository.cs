using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EFCGreenhouse.Repositories;

public class PlantRepository : IPlantRepository
{
    private readonly GreenhouseDbContext _context;

    public PlantRepository(GreenhouseDbContext context)
    {
        _context = context;
    }

    public async Task<Plant?> GetByIdAsync(int id)
    {
        return await _context.Set<Plant>().FindAsync(id);
    }

    public async Task<IEnumerable<Plant>> GetAllAsync()
    {
        return await _context.Set<Plant>().ToListAsync();
    }

    public async Task AddAsync(Plant plant)
    {
        await _context.Set<Plant>().AddAsync(plant);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Plant plant)
    {
        _context.Set<Plant>().Update(plant);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var plant = await GetByIdAsync(id);
        if (plant != null)
        {
            _context.Set<Plant>().Remove(plant);
            await _context.SaveChangesAsync();
        }
    }
}