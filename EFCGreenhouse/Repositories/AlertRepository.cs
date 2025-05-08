using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EFCGreenhouse.Repositories;

public class AlertRepository : IAlertRepository
{
    private readonly GreenhouseDbContext _context;

    public AlertRepository(GreenhouseDbContext context)
    {
        _context = context;
    }

    public async Task<Alert?> GetByIdAsync(int id)
    {
        return await _context.Set<Alert>().FindAsync(id);
    }

    public async Task<IEnumerable<Alert>> GetAllAsync()
    {
        return await _context.Set<Alert>().ToListAsync();
    }

    public async Task AddAsync(Alert alert)
    {
        await _context.Set<Alert>().AddAsync(alert);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Alert alert)
    {
        _context.Set<Alert>().Update(alert);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var alert = await GetByIdAsync(id);
        if (alert != null)
        {
            _context.Set<Alert>().Remove(alert);
            await _context.SaveChangesAsync();
        }
    }
}