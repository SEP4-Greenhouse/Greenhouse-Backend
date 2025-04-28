namespace Domain.Entities;

public class Sensor
{
    public int Id { get; private set; }
    public string Type { get; private set; }
    public string Status { get; private set; }
        
    public int GreenhouseId { get; private set; }
    public Greenhouse Greenhouse { get; private set; }
        
    public ICollection<SensorReading> Readings { get; private set; } = new List<SensorReading>();

    public Sensor(string type, string status, Greenhouse greenhouse)
    {
        Type = type;
        Status = status;
        Greenhouse = greenhouse;
        GreenhouseId = greenhouse.Id;
    }

    private Sensor() { } // Required by EF Core

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