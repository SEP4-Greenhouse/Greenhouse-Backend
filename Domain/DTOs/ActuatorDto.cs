namespace Domain.DTOs;

public class ActuatorDto(string type, string status)
{
    public string Type { get; set; } = type;
    public string Status { get; set; } = status;
}