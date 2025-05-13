namespace Domain.DTOs;

public class SensorDataDto
{
    public SensorData current { get; set; }
    public List<SensorData> history { get; set; }
}

public class SensorData
{
    public string SensorType { get; set; }
    public float Value { get; set; }
    public DateTime Timestamp { get; set; }
}