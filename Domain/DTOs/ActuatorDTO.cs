using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

public class ActuatorDTO
{
    public string Type { get; set; }
    public string Status { get; set; }
}