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
                .OrderByDescending(p => p.PredictionTimestamp)
                .ToListAsync();
        }

        public async Task<PredictionLog?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(PredictionLog log)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}