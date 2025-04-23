using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPredictionLogRepository
    {
        Task AddAsync(PredictionLog log);
        Task<IEnumerable<PredictionLog>> GetAllAsync();
        
    }
}