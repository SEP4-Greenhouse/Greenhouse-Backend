namespace Domain.IClients;

public interface IMqttListener
{
    Task StartListeningAsync();
    Task StopListeningAsync();
}