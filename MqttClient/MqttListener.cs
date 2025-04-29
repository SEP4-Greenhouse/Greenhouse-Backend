using System.Text;
using System.Text.Json;
using Domain.DTOs;
using Domain.Entities;
using Domain.IClients;
using EFCGreenhouse;
using MQTTnet;
using MQTTnet.Client;

namespace MqttClient;

public class MqttListener : IMqttListener
{
    private readonly GreenhouseDbContext _dbContext;
    private IMqttClient _mqttClient;

    public MqttListener(GreenhouseDbContext dbContext)
    {
        _dbContext = dbContext;
        _mqttClient = new MqttFactory().CreateMqttClient(); // Ensure MQTTnet is installed
    }

    public async Task StartListeningAsync()
    {
        var options = new MqttClientOptionsBuilder()
            .WithClientId("GreenhouseBackend")
            .WithTcpServer("host.docker.internal", 1883) // Adjust broker address if needed
            .WithCleanSession()
            .Build();

        _mqttClient.ApplicationMessageReceivedAsync += HandleMessageReceivedAsync;

        try
        {
            // Connect to the MQTT broker
            await _mqttClient.ConnectAsync(options, CancellationToken.None);
            Console.WriteLine("MQTT Client connected.");

            // Subscribe to the topic
            await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder()
                .WithTopic("greenhouse/sensors")
                .Build());
            Console.WriteLine("Subscribed to topic: greenhouse/sensors");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to MQTT broker: {ex.Message}");
        }
    }

    private async Task HandleMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
    {
        var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

        try
        {
            var sensorData = JsonSerializer.Deserialize<SensorDataDto>(payload);

            if (sensorData != null)
            {
                Console.WriteLine($"Received Sensor Data: {sensorData.SensorType} - {sensorData.Value} at {sensorData.Timestamp}");

                var reading = new SensorReading(
                    sensorData.Timestamp,
                    sensorData.Value,
                    sensorData.SensorType,
                    null // No Sensor reference yet
                );

                _dbContext.SensorReadings.Add(reading);
                await _dbContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to deserialize message: {ex.Message}");
        }
    }

    public async Task StopListeningAsync()
    {
        if (_mqttClient.IsConnected)
        {
            await _mqttClient.DisconnectAsync();
            Console.WriteLine("MQTT Client disconnected.");
        }
    }
}