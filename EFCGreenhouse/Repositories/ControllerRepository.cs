using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EFCGreenhouse.Repositories;

public class ControllerRepository : IControllerRepository
{
    private readonly GreenhouseDbContext _context;

    public ControllerRepository(GreenhouseDbContext context)
    {
        _context = context;
    }

    public async Task<Controller?> GetByIdAsync(int id)
    {
        return await _context.Set<Controller>().FindAsync(id);
    }

    public async Task<IEnumerable<Controller>> GetAllAsync()
    {
        return await _context.Set<Controller>().ToListAsync();
    }

    public async Task<IEnumerable<Controller>> GetByGreenhouseIdAsync(int greenhouseId)
    {
        return await _context.Set<Controller>()
            .Where(c => c.GreenhouseId == greenhouseId)
            .ToListAsync();
    }

    public async Task AddAsync(Controller controller)
    {
        await _context.Set<Controller>().AddAsync(controller);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Controller controller)
    {
        _context.Set<Controller>().Update(controller);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var controller = await GetByIdAsync(id);
        if (controller != null)
        {
            _context.Set<Controller>().Remove(controller);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsByIdAsync(int id)
    {
        return await _context.Set<Controller>().AnyAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<ControllerAction>> GetActionsByControllerIdAsync(int controllerId)
    {
        return await _context.Set<ControllerAction>()
            .Where(a => a.ControllerId == controllerId)
            .ToListAsync();
    }
}