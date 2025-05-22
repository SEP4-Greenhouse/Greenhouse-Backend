using Domain.Entities;
using Domain.IRepositories;

namespace EFCGreenhouse.Repositories;

public class PredictionLogRepository(GreenhouseDbContext context)
    : BaseRepository<PredictionLog>(context), IPredictionLogRepository
{
}