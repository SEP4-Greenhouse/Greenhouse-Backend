using Domain.Entities;

namespace Domain.DTOs;

public class SensorAlertDto
{
    public SensorReading SensorReading { get; set; }
    public string Message { get; set; }
}