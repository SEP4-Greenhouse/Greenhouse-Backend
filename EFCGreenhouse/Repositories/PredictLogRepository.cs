using Domain.Entities;
using Domain.IRepositories;
using EFCGreenhouse;
using EFCGreenhouse.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class PredictionLogRepository : BaseRepository<PredictionLog>, IPredictionLogRepository
{
    private readonly ILogger<PredictionLogRepository> _logger;

    public PredictionLogRepository(GreenhouseDbContext context, ILogger<PredictionLogRepository> logger) 
        : base(context)
    {
        _logger = logger;
    }

    public override async Task<IEnumerable<PredictionLog>> GetAllAsync()
    {
        return await DbSet
            .AsNoTracking()
            .OrderByDescending(p => p.Timestamp)
            .ToListAsync();
    }
}