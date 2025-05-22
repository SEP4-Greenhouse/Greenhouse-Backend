using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EFCGreenhouse.Repositories;

public class SensorRepository(GreenhouseDbContext context) : BaseRepository<Sensor>(context), ISensorRepository
{
    public async Task<IEnumerable<Sensor>> GetByGreenhouseIdAsync(int greenhouseId)
    {
        return await Context.Sensors
            .Where(s => s.GreenhouseId == greenhouseId)
            .ToListAsync();
    }
}