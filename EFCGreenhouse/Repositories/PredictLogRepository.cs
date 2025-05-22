using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EFCGreenhouse.Repositories;

public class PredictionLogRepository(GreenhouseDbContext context)
    : BaseRepository<PredictionLog>(context), IPredictionLogRepository
{
    public override async Task<IEnumerable<PredictionLog>> GetAllAsync()
    {
        return await DbSet
            .AsNoTracking()
            .OrderByDescending(p => p.PredictionTime)
            .ToListAsync();
    }

    public async Task<PredictionLog> AddAsync(PredictionLog log)
    {
        await DbSet.AddAsync(log);
        await Context.SaveChangesAsync();
        return log;
    }
}