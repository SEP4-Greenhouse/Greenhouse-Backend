using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

public class MlSensorReadingDto
{
    public MlSensorReadingDto(string sensorName, string unit, double value)
    {
        SensorName = sensorName;
        Unit = unit;
        Value = value;
    }
    public MlSensorReadingDto()
    {
    }

    [Required]
    public string SensorName { get; set; }
    [Required]
    public string Unit { get; set; }
    [Required]
    public double Value { get; set; }
}
