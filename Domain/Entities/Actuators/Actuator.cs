using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities.Actuators;

public abstract class Actuator
{
    [Key]
    public int Id { get; private set; }

    [Required] [MaxLength(100)] public string Type => GetType().Name;

    [Required] [MaxLength(100)] public string Status { get; private set; }

    [ForeignKey("Greenhouse")] public int GreenhouseId { get; private set; }

    public Greenhouse Greenhouse { get; private set; }

    public List<ActuatorAction> Actions { get; private set; }

    // Parameterless constructor for EF Core
    [JsonConstructor]
    protected Actuator()
    {
        Status = string.Empty;
        Actions = new List<ActuatorAction>();
    }

    protected Actuator(string status, Greenhouse greenhouse)
    {
        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("Status cannot be empty.");
            
        Status = status;
        Greenhouse = greenhouse ?? throw new ArgumentNullException(nameof(greenhouse));
        GreenhouseId = greenhouse.Id;
        Actions = new List<ActuatorAction>();
    }

    public void UpdateStatus(string newStatus)
    {
        if (string.IsNullOrWhiteSpace(newStatus))
            throw new ArgumentException("Status cannot be empty.");
        Status = newStatus;
    }

    public ActuatorAction InitiateAction(DateTime timestamp, string type, double value)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("ActuatorAction type cannot be empty.");

        var action = new ActuatorAction(timestamp, type, value, this);
        Actions.Add(action);
        return action;
    }
        
    public virtual ActuatorAction TurnOn()
    {
        UpdateStatus("On");
        return InitiateAction(DateTime.Now, "TurnOn", 1);
    }
    
    public virtual ActuatorAction TurnOff()
    {
        UpdateStatus("Off");
        return InitiateAction(DateTime.Now, "TurnOff", 0);
    }
}