using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Sensor
{
    [Key]
     // Disable auto-generation of IDs
    public int Id { get; set; } // Made publicly settable to allow custom ID assignment

    [Required] [MaxLength(100)] public string Type { get; private set; }

    [Required] [MaxLength(100)] public string Status { get; private set; }

    [Required] [MaxLength(50)] public string Unit { get; private set; }
    [ForeignKey("Greenhouse")] public int GreenhouseId { get; private set; }

    public Greenhouse Greenhouse { get; private set; }

    public ICollection<SensorReading> Readings { get; private set; } = new List<SensorReading>();

    public Sensor(string type, string status, string unit, Greenhouse greenhouse)
    {
        Type = type;
        Status = status;
        Unit = unit ?? throw new ArgumentNullException(nameof(unit));
        Greenhouse = greenhouse ?? throw new ArgumentNullException(nameof(greenhouse));
        GreenhouseId = greenhouse.Id;
    }

    private Sensor()
    {
    } // Required by EF Core

    public void UpdateStatus(string newStatus)
    {
        if (string.IsNullOrWhiteSpace(newStatus))
            throw new ArgumentException("Status cannot be empty.");
        Status = newStatus;
    }

    public SensorReading AddReading(DateTime timestamp, double value, string unit)
    {
        var reading = new SensorReading(timestamp, value, unit, this);
        Readings.Add(reading);
        return reading;
    }
}