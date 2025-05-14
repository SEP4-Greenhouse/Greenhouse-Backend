using Domain.Entities;

namespace Domain.IClients
{
    public interface IIoTClient
    {
        Task<List<SensorReading>> GetAllSensorDataByDateRangeAsync(DateTime start, DateTime end);
        Task<List<SensorReading>> GetLatestSensorDataAsync();
        Task<SensorReading> GetLatestSensorDataBySensorAsync(int sensorId);
        Task<List<SensorReading>> GetAllSensorDataBySensorAsync(int sensorId);
        Task<List<SensorReading>> GetSensorDataBySensorAndDateRangeAsync(int sensorId, DateTime start, DateTime end);
        Task<bool> SendCommandToControllerAsync(ActuatorAction actuatorAction);
        Task<string> GetControllerStatusAsync(int controllerId);
        Task<bool> UpdateControllerStatusAsync(int controllerId, string newStatus);
    }
}