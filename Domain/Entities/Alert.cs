using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Alert
{
    [Key] public int Id { get; private set; }
    [Required] [MaxLength(100)] public string Type { get; private set; }
    [Required] public string Message { get; private set; }
    public IReadOnlyCollection<SensorReading> TriggeringSensorReadings { get; private set; }
    public IReadOnlyCollection<Action> TriggeringActions { get; private set; }

    public Alert(string type, string message)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Type cannot be empty.");
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be empty.");

        Type = type;
        Message = message;
        TriggeringSensorReadings = new List<SensorReading>();
        TriggeringActions = new List<Action>();
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
}