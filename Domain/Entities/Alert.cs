using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Alert
{
    public enum AlertType
    {
        Sensor,
        Actuator,
    }

    [Key] public int Id { get; private set; }

    [Required] public AlertType Type { get; private set; }

    [Required] public string Message { get; private set; }
    
    [Required] public DateTime Timestamp { get; private set; }

    private readonly List<SensorReading> _triggeringSensorReadings = new();
    private readonly List<ActuatorAction> _triggeringActions = new();

    public IReadOnlyCollection<SensorReading> TriggeringSensorReadings => _triggeringSensorReadings.AsReadOnly();
    public IReadOnlyCollection<ActuatorAction> TriggeringActions => _triggeringActions.AsReadOnly();

    public Alert(AlertType type, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be empty.");

        Type = type;
        Message = message;
        Timestamp = DateTime.UtcNow;
    }

    private Alert()
    {
    }

    public void UpdateMessage(string newMessage)
    {
        if (string.IsNullOrWhiteSpace(newMessage))
            throw new ArgumentException("Message cannot be empty.");
        Message = newMessage;
    }

    public void AddTriggeringSensorReading(SensorReading reading)
    {
        if (reading == null)
            throw new ArgumentNullException(nameof(reading));
            
        _triggeringSensorReadings.Add(reading);
    }

    public void AddTriggeringActuatorAction(ActuatorAction action)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));
            
        _triggeringActions.Add(action);
    }
}