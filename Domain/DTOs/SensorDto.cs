namespace Domain.DTOs;

public class SensorDto(string type, string status, string unit)
{
    public string Type { get; set; } = type;
    public string Status { get; set; } = status;

    public string Unit { get; set; } = unit;
}