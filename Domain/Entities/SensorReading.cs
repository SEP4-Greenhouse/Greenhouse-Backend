namespace Domain.Entities;

public class SensorReading
{
    public int Id { get; private set; }
    public DateTime TimeStamp { get; private set; }
    public double Value { get; private set; }
    public string Unit { get; private set; }
        
    public int SensorId { get; private set; }
    public Sensor Sensor { get; private set; }
        
    public ICollection<Alert> TriggeredAlerts { get; private set; } = new List<Alert>();
    public ICollection<Plant> AffectedPlants { get; private set; } = new List<Plant>();

    public SensorReading(DateTime timestamp, double value, string unit, Sensor sensor)
    {
        TimeStamp = timestamp;
        Value = value;
        Unit = unit;
        Sensor = sensor;
        SensorId = sensor.Id;
    }

    private SensorReading() { } // Required by EF Core

    public void AddAffectedPlant(Plant plant)
    {
        AffectedPlants.Add(plant);
    }

    public Alert TriggerAlert(string type, string message)
    {
        var alert = new Alert(type, message);
        TriggeredAlerts.Add(alert);
        return alert;
    }
}