namespace Domain.DTOs;

public class SensorDataDto
{
    public string SensorType { get; init; }
    public double Value { get; init; }
    public DateTime Timestamp { get; init; }

    public SensorDataDto(string sensorType, double value, DateTime timestamp)
    {
        if (string.IsNullOrWhiteSpace(sensorType))
            throw new ArgumentException("SensorType cannot be empty.");
        
        SensorType = sensorType;
        Value = value;
        Timestamp = timestamp;
    }
}