using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Action
{
    [Key] public int Id { get; private set; }

    [Required] public DateTime Timestamp { get; private set; }

    [Required] [MaxLength(100)] public string Type { get; private set; }
    [Required] public double Value { get; private set; }
    [ForeignKey("Controller")] public int ControllerId { get; private set; }
    public Controller Controller { get; private set; }
    public IReadOnlyCollection<Alert> TriggeredAlerts { get; private set; }

    public Action(DateTime timestamp, string type, double value, Controller controller)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Type cannot be empty.");

        Timestamp = timestamp;
        Type = type;
        Value = value;
        Controller = controller ?? throw new ArgumentNullException(nameof(controller));
        ControllerId = controller.Id;
        TriggeredAlerts = new List<Alert>();
    }

    private Action()
    {
    }

    public Alert TriggerAlert(string type, string message)
    {
        if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Type and message cannot be empty.");

        var alert = new Alert(type, message);
        ((List<Alert>)TriggeredAlerts).Add(alert);
        return alert;
    }
}