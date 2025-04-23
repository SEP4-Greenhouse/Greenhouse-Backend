using Microsoft.Extensions.Hosting;
using MQTTnet;

namespace MqttClient;

public class MqttClient : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var Factory = new MqttClientFactory();
        var mqttClient = Factory.CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithClientId("GreenhouseApi")
            .WithTcpServer("localhost", 1883)
            .Build();
    }
}