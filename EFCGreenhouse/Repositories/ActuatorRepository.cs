using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EFCGreenhouse.Repositories;

public class ActuatorRepository(GreenhouseDbContext context)
    : BaseRepository<Actuator>(context), IActuatorRepository
{
    public async Task<IEnumerable<Actuator>> GetByGreenhouseIdAsync(int greenhouseId)
    {
        return await DbSet
            .Where(c => c.GreenhouseId == greenhouseId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ActuatorAction>> GetActionsByActuatorIdAsync(int ActuatorId)
    {
        return await Context.ActuatorActions
            .Where(a => a.ActuatorId == ActuatorId)
            .ToListAsync();
    }
}