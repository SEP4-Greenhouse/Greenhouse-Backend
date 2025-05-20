// IotClient/IotClientSimulator/IotSimulator.cs
// Simulation logic developed collaboratively with ChatGPT (OpenAI) for educational use
using System.Text;
using System.Text.Json;
using Domain.Entities;

namespace IotClient.IotClientSimulator;

public class IotSimulator
{
    private readonly HttpClient _httpClient;
    private readonly Timer _timer;
    private readonly Dictionary<string, float> _currentSensorValues;
    private readonly string _backendSensorUrl = "http://localhost:5001/api/sensor/sensor/reading(IOT)";
    private readonly string _backendActuatorUrl = "http://localhost:5001/api/actuator/{0}/action";
    private readonly Greenhouse _greenhouse;

    public IotSimulator()
    {
        _httpClient = new HttpClient();

        // Simulate a greenhouse to associate with sensors
        _greenhouse = new Greenhouse("SimulatedGreenhouse", new User("SimUser", "iot@example.com", "pass"));
        typeof(Greenhouse).GetProperty("Id")?.SetValue(_greenhouse, 1);

        // Initial simulated values for each sensor type
        _currentSensorValues = new Dictionary<string, float>
        {
            ["AirTemperature"] = 25.0f,
            ["AirHumidity"] = 50.0f,
            ["SoilHumidity"] = 35.0f,
            ["Light"] = 15000.0f,
            ["Proximity"] = 0f,
            ["PIR"] = 0f,
            ["CO2"] = 400.0f
        };

        // Setup the timer to trigger simulation every 10 seconds
        _timer = new Timer(async _ => await RunSimulationAsync(), null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
    }

    // Main simulation runner
    private async Task RunSimulationAsync()
    {
        SimulateEnvironmentalShift();  // Update values based on environment
        await SendSensorReadingsAsync();  // Push sensor values to backend
        await NotifyBackendToEvaluateActuatorsAsync();  // Log info (backend logic only)
    }

    // Simulate environmental dynamics such as day/night temperature and soil drying
    private void SimulateEnvironmentalShift()
    {
        bool isDaytime = DateTime.UtcNow.Hour is >= 6 and <= 18;

        // Air temperature: warmer in the day, cooler at night
        _currentSensorValues["AirTemperature"] += isDaytime ? 0.2f : -0.2f;
        _currentSensorValues["AirTemperature"] = Math.Clamp(_currentSensorValues["AirTemperature"], 10f, 40f);

        // Soil humidity: gradually dries
        _currentSensorValues["SoilHumidity"] -= 0.5f;
        _currentSensorValues["SoilHumidity"] = Math.Max(_currentSensorValues["SoilHumidity"], 0f);

        // Apply small random drift to other sensors
        foreach (var key in _currentSensorValues.Keys.ToList())
        {
            if (key is "AirTemperature" or "SoilHumidity") continue;
            float delta = RandomFloat(-0.5f, 0.5f);
            _currentSensorValues[key] = Math.Clamp(_currentSensorValues[key] + delta, 0, 100000);
        }
    }

    // Converts simulated sensor values into SensorReading objects and sends them to the backend
    private async Task SendSensorReadingsAsync()
    {
        var now = DateTime.UtcNow;

        foreach (var entry in _currentSensorValues)
        {
            var sensor = new Sensor(entry.Key, "active", _greenhouse);
            var reading = new SensorReading(now, entry.Value, GetUnit(entry.Key), sensor);
            await PostJsonAsync(_backendSensorUrl, reading);
        }
    }

    // This function does not call any actuator control directly, it just logs info
    private async Task NotifyBackendToEvaluateActuatorsAsync()
    {
        Console.WriteLine("[INFO] Trigger condition check sent to backend (via sensor readings)");
    }

    // Helper function to send any object as JSON to the specified backend endpoint
    private async Task PostJsonAsync<T>(string url, T payload)
    {
        try
        {
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
                Console.WriteLine($"[ERROR] POST {url} failed: {response.StatusCode}");
            else
                Console.WriteLine($"[OK] POST {url} => {typeof(T).Name}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EXCEPTION] {ex.Message}");
        }
    }

    // Maps sensor types to units for frontend/backend use
    private static string GetUnit(string type) => type.ToLower() switch
    {
        "airtemperature" => "°C",
        "airhumidity" => "%",
        "soilhumidity" => "%",
        "light" => "lux",
        "proximity" => "cm",
        "pir" => "trigger",
        "co2" => "ppm",
        _ => "units"
    };

    // Simple float randomizer helper
    private static float RandomFloat(float min, float max)
    {
        var rand = new Random();
        return (float)(min + rand.NextDouble() * (max - min));
    }
}
