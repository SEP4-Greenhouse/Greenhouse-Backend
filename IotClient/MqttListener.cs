using System.Text;
using System.Text.Json;
using Domain.DTOs;
using Domain.Entities;
using Domain.IClients;
using EFCGreenhouse;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

namespace IotClient;

public class MqttListener : IMqttListener
{
    private readonly GreenhouseDbContext _dbContext;
    private readonly IMqttClient _client;
    private readonly MqttClientOptions _options;

    public MqttListener(GreenhouseDbContext dbContext)
    {
        _dbContext = dbContext;

        var factory = new MqttFactory();
        _client = factory.CreateMqttClient();

        _options = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", 1883) // Change if broker is remote
            .WithCleanSession()
            .Build();

        // Event: When connected
        _client.ConnectedAsync += async e =>
        {
            Console.WriteLine("✅ Connected to MQTT broker.");

            await _client.SubscribeAsync(new MqttTopicFilterBuilder()
                .WithTopic("greenhouse/sensors")
                .WithAtMostOnceQoS()
                .Build());

            Console.WriteLine("📡 Subscribed to topic: greenhouse/sensors");
        };

        // Event: When disconnected
        _client.DisconnectedAsync += e =>
        {
            Console.WriteLine("⚠️ Disconnected from MQTT broker.");
            return Task.CompletedTask;
        };

        // Event: When a message is received
        _client.ApplicationMessageReceivedAsync += async e =>
        {
            var topic = e.ApplicationMessage.Topic;
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload ?? Array.Empty<byte>());

            Console.WriteLine($"📥 Received message - Topic: {topic} | Message: {payload}");

            try
            {
                var sensorData = JsonSerializer.Deserialize<SensorDataDto>(payload);

                if (sensorData != null)
                {
                    var reading = new SensorReading(
                        sensorData.current.Timestamp,
                        sensorData.current.Value,
                        sensorData.current.SensorType,
                        null // TODO: Link to Sensor entity if available§
                    );

                    _dbContext.SensorReadings.Add(reading);
                    await _dbContext.SaveChangesAsync();

                    Console.WriteLine("✅ Sensor reading saved to database.");
                }
                else
                {
                    Console.WriteLine("⚠️ Deserialized data was null.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to deserialize or save message: {ex.Message}");
            }
        };
    }

    public async Task StartListeningAsync()
    {
        try
        {
            if (!_client.IsConnected)
            {
                await _client.ConnectAsync(_options);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ MQTT connection failed: {ex.Message}");
        }
    }

    public async Task StopListeningAsync()
    {
        if (_client.IsConnected)
        {
            await _client.DisconnectAsync();
            Console.WriteLine("🔌 MQTT Client disconnected.");
        }
    }
}
