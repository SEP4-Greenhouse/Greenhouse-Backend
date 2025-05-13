using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EFCGreenhouse.Repositories;

public class ControllerRepository(GreenhouseDbContext context)
    : BaseRepository<Controller>(context), IControllerRepository
{
    public async Task<IEnumerable<Controller>> GetByGreenhouseIdAsync(int greenhouseId)
    {
        return await DbSet
            .Where(c => c.GreenhouseId == greenhouseId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ControllerAction>> GetActionsByControllerIdAsync(int controllerId)
    {
        return await Context.ControllerActions
            .Where(a => a.ControllerId == controllerId)
            .ToListAsync();
    }
}