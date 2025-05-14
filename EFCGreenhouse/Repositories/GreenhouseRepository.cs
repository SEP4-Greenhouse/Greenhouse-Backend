using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EFCGreenhouse.Repositories;

public class GreenhouseRepository(GreenhouseDbContext context)
    : BaseRepository<Greenhouse>(context), IGreenhouseRepository
{
    public async Task<IEnumerable<Greenhouse>> GetByUserIdAsync(int userId)
    {
        return await context.Greenhouses
            .Where(g => g.UserId == userId)
            .ToListAsync();
    }
}