using Domain.Entities;

namespace Domain.DTOs;

public class ActuatorAlertDto
{
    public ActuatorAction ActuatorAction { get; set; }
    public string Message { get; set; }
}