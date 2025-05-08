using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EFCGreenhouse.Repositories;

public class GreenhouseRepository : IGreenhouseRepository
{
    private readonly GreenhouseDbContext _context;

    public GreenhouseRepository(GreenhouseDbContext context)
    {
        _context = context;
    }

    public async Task<Greenhouse?> GetByIdAsync(int id)
    {
        return await _context.Set<Greenhouse>().FindAsync(id);
    }

    public async Task<IEnumerable<Greenhouse>> GetAllAsync()
    {
        return await _context.Set<Greenhouse>().ToListAsync();
    }

    public async Task AddAsync(Greenhouse greenhouse)
    {
        await _context.Set<Greenhouse>().AddAsync(greenhouse);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Greenhouse greenhouse)
    {
        _context.Set<Greenhouse>().Update(greenhouse);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var greenhouse = await GetByIdAsync(id);
        if (greenhouse != null)
        {
            _context.Set<Greenhouse>().Remove(greenhouse);
            await _context.SaveChangesAsync();
        }
    }
}