using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EFCGreenhouse.Repositories
{
    public class PredictionLogRepository : IPredictionLogRepository
    {
        private readonly GreenhouseDbContext _context;

        public PredictionLogRepository(GreenhouseDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PredictionLog log)
        {
            await _context.PredictionLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PredictionLog>> GetAllAsync()
        {
            return await _context.PredictionLogs
                .OrderByDescending(p => p.Timestamp) // Use the correct property
                .ToListAsync();
        }

        public async Task<PredictionLog?> GetByIdAsync(int id)
        {
            return await _context.PredictionLogs.FindAsync(id);
        }

        public async Task UpdateAsync(PredictionLog log)
        {
            _context.PredictionLogs.Update(log);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var log = await _context.PredictionLogs.FindAsync(id);
            if (log != null)
            {
                _context.PredictionLogs.Remove(log);
                await _context.SaveChangesAsync();
            }
        }
    }
}