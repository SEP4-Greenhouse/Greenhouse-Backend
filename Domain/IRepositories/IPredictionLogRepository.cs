using Domain.Entities;

namespace Domain.IRepositories
{
    public interface IPredictionLogRepository
    {
        Task AddAsync(PredictionLog log);
        Task<IEnumerable<PredictionLog>> GetAllAsync();
        Task<PredictionLog?> GetByIdAsync(int id);
        Task UpdateAsync(PredictionLog log);
        Task DeleteAsync(int id);
    }
}