using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

public class ActuatorActionDto(double value, string action)
{
    [Required]
    public DateTime Timestamp { get; set; }

    [Required]
    public string Action { get; set; } = action;

    [Required]
    public double Value { get; set; } = value;
}