namespace Domain.DTOs;

public class ActuatorActionDto
{
    public DateTime Timestamp { get; set; }

    public string Type { get; set; }

    public double Value { get; set; }
}