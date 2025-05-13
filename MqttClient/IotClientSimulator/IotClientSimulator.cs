using Domain.Entities;
using Domain.IClients;

namespace MqttClient.IotClientSimulator;

public class IotClientSimulator : IIoTClient
{
    public async Task<List<SensorReading>> GetAllSensorDataByDateRangeAsync(DateTime start, DateTime end)
    {
        throw new NotImplementedException();
    }

    public async Task<List<SensorReading>> GetLatestSensorDataAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<SensorReading> GetLatestSensorDataBySensorAsync(int sensorId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<SensorReading>> GetAllSensorDataBySensorAsync(int sensorId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<SensorReading>> GetSensorDataBySensorAndDateRangeAsync(int sensorId, DateTime start, DateTime end)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SendCommandToControllerAsync(ControllerAction controllerAction)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetControllerStatusAsync(int controllerId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateControllerStatusAsync(int controllerId, string newStatus)
    {
        throw new NotImplementedException();
    }
}