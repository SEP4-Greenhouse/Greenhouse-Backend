using Domain.Entities;

namespace Domain.IRepositories;

public interface IThresholdRepository
{
    Task<Threshold> GetThresholdBySensorIdAsync(int sensorId);
    Task<IEnumerable<Threshold>> GetAllThresholdsAsync();
    Task<Threshold> AddThresholdAsync(Threshold threshold);
    Task<Threshold> UpdateThresholdAsync(Threshold threshold);
    Task<bool> DeleteThresholdAsync(int id);
}