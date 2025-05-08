using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EFCGreenhouse.Repositories;

public class ActionRepository : IActionRepository
{
    private readonly GreenhouseDbContext _context;

    public ActionRepository(GreenhouseDbContext context)
    {
        _context = context;
    }

    public async Task<ControllerAction> GetByIdAsync(int id)
    {
        return await _context.Set<ControllerAction>().FindAsync(id);
    }

    public async Task<IEnumerable<ControllerAction>> GetAllAsync()
    {
        return await _context.Set<ControllerAction>().ToListAsync();
    }

    public async Task AddAsync(ControllerAction controllerAction)
    {
        await _context.Set<ControllerAction>().AddAsync(controllerAction);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ControllerAction controllerAction)
    {
        _context.Set<ControllerAction>().Update(controllerAction);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var action = await GetByIdAsync(id);
        if (action != null)
        {
            _context.Set<ControllerAction>().Remove(action);
            await _context.SaveChangesAsync();
        }
    }
}