using System;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using IDomainMqttClient = Domain.IClients.IMqttClient;

namespace MqttClient;

public class MqttClient : IDomainMqttClient
{
    private readonly IMqttClient _client;
    private readonly MqttClientOptions _options;

    public MqttClient()
    {
        var factory = new MqttFactory();
        _client = factory.CreateMqttClient();

        _options = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", 1883)
            .WithCleanSession()
            .Build();

        _client.ConnectedAsync += async e =>
        {
            Console.WriteLine("✅ Connected to MQTT broker.");

            await _client.SubscribeAsync(new MqttTopicFilterBuilder()
                .WithTopic("greenhouse/sensor/temp")
                .WithAtMostOnceQoS()
                .Build());

            Console.WriteLine("📡 Subscribed to topic: greenhouse/sensor/temp");
        };

        _client.DisconnectedAsync += e =>
        {
            Console.WriteLine("⚠️ Disconnected from MQTT broker.");
            return Task.CompletedTask;
        };

        _client.ApplicationMessageReceivedAsync += e =>
        {
            var topic = e.ApplicationMessage.Topic;
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload ?? Array.Empty<byte>());
            Console.WriteLine($"📥 Received message - Topic: {topic} | Message: {payload}");
            return Task.CompletedTask;
        };
    }

    public async Task ConnectAsync()
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
}
