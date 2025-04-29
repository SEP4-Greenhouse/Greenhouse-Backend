namespace Domain.IClients;

public interface IMqttClient
{
    Task ConnectAsync();
}
