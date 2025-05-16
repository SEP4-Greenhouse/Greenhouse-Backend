using Domain.Entities;

namespace Domain.DTOs;

public class SensorReadingDto
{
    public DateTime TimeStamp { get; set; }

    public double Value { get; set; }
    
    public string Unit { get; set; }
}
