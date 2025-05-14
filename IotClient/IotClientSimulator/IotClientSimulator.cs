using Domain.Entities;
using Domain.IClients;

namespace IotClient.IotClientSimulator;

public class IotClientSimulator : IIoTClient
{
    private readonly List<SensorReading> _mockReadings;
    private readonly List<Actuator> _mockControllers;
    private readonly Dictionary<int, string> _controllerStatuses;
    private readonly Random _random = new();

    // Explicit IDs for all entities
    private const int UserId = 1;
    private const int GreenhouseId = 101;
    private const int TemperatureSensorId = 201;
    private const int HumiditySensorId = 202;
    private const int LightSensorId = 203;
    private const int WaterPumpControllerId = 301;

    public IotClientSimulator()
    {
        // Create mock user first (required for greenhouse)
        var user = new User("MockUser", "user@example.com", "hashedpassword");
        typeof(User).GetProperty("Id")?.SetValue(user, UserId);

        // Initialize a single mock greenhouse with explicit ID
        var greenhouse = new Greenhouse("Smart Greenhouse", user);
        typeof(Greenhouse).GetProperty("Id")?.SetValue(greenhouse, GreenhouseId);

        // Initialize mock sensors with explicit IDs
        List<Sensor> mockSensors = new List<Sensor>
        {
            new Sensor(TemperatureSensorId, "temperature", "active", greenhouse),
            new Sensor(HumiditySensorId, "humidity", "active", greenhouse),
            new Sensor(LightSensorId, "light", "active", greenhouse)
        };

        // Initialize mock readings
        _mockReadings = new List<SensorReading>();
        var now = DateTime.UtcNow;

        // Generate readings for the past 7 days
        for (int day = 0; day < 7; day++)
        {
            for (int hour = 0; hour < 24; hour += 3) // Every 3 hours
            {
                var timestamp = now.AddDays(-day).AddHours(-hour);

                foreach (var sensor in mockSensors)
                {
                    double value = sensor.Type.ToLower() switch
                    {
                        "temperature" => _random.Next(18, 32) + _random.NextDouble(),
                        "humidity" => _random.Next(40, 85) + _random.NextDouble(),
                        "light" => _random.Next(5000, 20000) + _random.NextDouble(),
                        _ => _random.Next(0, 100) + _random.NextDouble()
                    };

                    string unit = sensor.Type.ToLower() switch
                    {
                        "temperature" => "°C", 
                        "humidity" => "%",
                        "light" => "lux",
                        _ => "units"
                    };

                    _mockReadings.Add(new SensorReading(timestamp, value, unit, sensor));
                }
            }
        }

        // Initialize mock actuator with explicit ID
        _mockControllers = new List<Actuator>
        {
            new WaterPumpActuator(WaterPumpControllerId, "active", greenhouse)
        };

        // Initialize actuator status with explicit ID
        _controllerStatuses = new Dictionary<int, string>
        {
            { WaterPumpControllerId, "active" }
        };
    }

    public async Task<List<SensorReading>> GetAllSensorDataByDateRangeAsync(DateTime start, DateTime end)
    {
        return await Task.FromResult(_mockReadings
            .Where(r => r.TimeStamp >= start && r.TimeStamp <= end)
            .ToList());
    }

    public async Task<List<SensorReading>> GetLatestSensorDataAsync()
    {
        return await Task.FromResult(_mockReadings
            .GroupBy(r => r.SensorId)
            .Select(g => g.OrderByDescending(r => r.TimeStamp).First())
            .ToList());
    }

    public async Task<SensorReading> GetLatestSensorDataBySensorAsync(int sensorId)
    {
        return await Task.FromResult(_mockReadings
            .Where(r => r.SensorId == sensorId)
            .OrderByDescending(r => r.TimeStamp)
            .First());
    }

    public async Task<List<SensorReading>> GetAllSensorDataBySensorAsync(int sensorId)
    {
        return await Task.FromResult(_mockReadings
            .Where(r => r.SensorId == sensorId)
            .ToList());
    }

    public async Task<List<SensorReading>> GetSensorDataBySensorAndDateRangeAsync(int sensorId, DateTime start, DateTime end)
    {
        return await Task.FromResult(_mockReadings
            .Where(r => r.SensorId == sensorId && r.TimeStamp >= start && r.TimeStamp <= end)
            .ToList());
    }

    public async Task<bool> SendCommandToControllerAsync(ActuatorAction actuatorAction)
    {
        // Simulate successful command execution
        return await Task.FromResult(true);
    }

    public async Task<string> GetControllerStatusAsync(int controllerId)
    {
        if (_controllerStatuses.TryGetValue(controllerId, out var status))
        {
            return await Task.FromResult(status);
        }
        return await Task.FromResult("unknown");
    }

    public async Task<bool> UpdateControllerStatusAsync(int controllerId, string newStatus)
    {
        if (_controllerStatuses.ContainsKey(controllerId))
        {
            _controllerStatuses[controllerId] = newStatus;
            return await Task.FromResult(true);
        }
        return await Task.FromResult(false);
    }
}