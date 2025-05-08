using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using Action = Domain.Entities.Action;

namespace EFCGreenhouse.Repositories;

public class ActionRepository : IActionRepository
{
    private readonly GreenhouseDbContext _context;

    public ActionRepository(GreenhouseDbContext context)
    {
        _context = context;
    }

    public async Task<Action> GetByIdAsync(int id)
    {
        return await _context.Set<Action>().FindAsync(id);
    }

    public async Task<IEnumerable<Action>> GetAllAsync()
    {
        return await _context.Set<Action>().ToListAsync();
    }

    public async Task AddAsync(Action action)
    {
        await _context.Set<Action>().AddAsync(action);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Action action)
    {
        _context.Set<Action>().Update(action);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var action = await GetByIdAsync(id);
        if (action != null)
        {
            _context.Set<Action>().Remove(action);
            await _context.SaveChangesAsync();
        }
    }
}