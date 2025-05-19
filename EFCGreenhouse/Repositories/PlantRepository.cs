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
}