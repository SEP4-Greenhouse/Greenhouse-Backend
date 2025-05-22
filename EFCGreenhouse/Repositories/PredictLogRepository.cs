using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EFCGreenhouse.Repositories;

public class PredictionLogRepository(GreenhouseDbContext context)
    : BaseRepository<PredictionLog>(context), IPredictionLogRepository
{
}