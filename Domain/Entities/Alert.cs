using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Alert
{
    public enum AlertType
    {
        Sensor,
        Controller,
        Manual,
        System,
        Unknown
    }

    [Key] public int Id { get; private set; }

    [Required] public AlertType Type { get; private set; }

    [Required] public string Message { get; private set; }

    // Backing fields for EF Core
    private readonly List<SensorReading> _triggeringSensorReadings = new();
    private readonly List<ControllerAction> _triggeringActions = new();

    // Read-only collections that expose the backing fields
    public IReadOnlyCollection<SensorReading> TriggeringSensorReadings => _triggeringSensorReadings.AsReadOnly();
    public IReadOnlyCollection<ControllerAction> TriggeringActions => _triggeringActions.AsReadOnly();

    public Alert(AlertType type, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be empty.");

        Type = type;
        Message = message;
    }

    // Parameterless constructor required by EF Core
    private Alert()
    {
    }

    public void UpdateMessage(string newMessage)
    {
        if (string.IsNullOrWhiteSpace(newMessage))
            throw new ArgumentException("Message cannot be empty.");
        Message = newMessage;
    }

    // Methods to add related entities
    public void AddTriggeringSensorReading(SensorReading reading)
    {
        if (reading == null)
            throw new ArgumentNullException(nameof(reading));
            
        _triggeringSensorReadings.Add(reading);
    }

    public void AddTriggeringAction(ControllerAction action)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));
            
        _triggeringActions.Add(action);
    }
}