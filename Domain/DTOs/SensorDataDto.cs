namespace Domain.DTOs;

public class SensorDataDto
{
    public string SensorType { get; set; }
    public double Value { get; set; }
    public DateTime Timestamp { get; set; }
}