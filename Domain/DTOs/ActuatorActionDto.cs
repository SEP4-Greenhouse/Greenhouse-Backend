namespace Domain.DTOs;

public class ActuatorActionDto
{
    public DateTime Timestamp { get; set; }

    public string Action { get; set; }

    public double Value { get; set; }
}