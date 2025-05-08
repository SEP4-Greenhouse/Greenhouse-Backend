using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class SensorReading
{
    [Key] public int Id { get; private set; }

    [Required] public DateTime TimeStamp { get; private set; }

    [Required] public double Value { get; private set; }

    [Required] [MaxLength(50)] public string Unit { get; private set; }

    [ForeignKey("Sensor")] public int SensorId { get; private set; }
    public Sensor Sensor { get; private set; }

    public ICollection<Alert> TriggeredAlerts { get; private set; } = new List<Alert>();
    public ICollection<Plant> AffectedPlants { get; private set; } = new List<Plant>();

    public SensorReading(DateTime timestamp, double value, string unit, Sensor sensor)
    {
        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("Unit cannot be empty.");

        TimeStamp = timestamp;
        Value = value;
        Unit = unit;
        Sensor = sensor ?? throw new ArgumentNullException(nameof(sensor));
        SensorId = sensor.Id;
    }

    private SensorReading()
    {
    } // EF Core

    public void AddAffectedPlant(Plant plant)
    {
        AffectedPlants.Add(plant);
    }

    public Alert TriggerAlert(Alert.AlertType alertType, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be empty.");

        var alert = new Alert(alertType, message);
        TriggeredAlerts.Add(alert);
        return alert;
    }
}