using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EFCGreenhouse.Repositories;

public class PlantRepository(GreenhouseDbContext context) : BaseRepository<Plant>(context), IPlantRepository
{
    public async Task<IEnumerable<Plant>> GetByGreenhouseIdAsync(int greenhouseId)
    {
        return await context.Plants
            .Where(p => p.GreenhouseId == greenhouseId)
            .ToListAsync();
    }

    public override async Task<Plant?> GetByIdAsync(int id)
    {
        return await Context.Plants
            .Include(p => p.Greenhouse)
            .ThenInclude(g => g.Sensors)
            .Include(p => p.Greenhouse)
            .ThenInclude(g => g.Actuators)
            .ThenInclude(a => a.Actions)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}