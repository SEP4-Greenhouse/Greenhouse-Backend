using Domain.Entities;
using Domain.IRepositories;
using EFCGreenhouse.Repositories;

namespace EFCGreenhouse.Repositories;

public class SensorRepository : BaseRepository<Sensor>, ISensorRepository
{
    public SensorRepository(GreenhouseDbContext context) : base(context)
    {
    }
}