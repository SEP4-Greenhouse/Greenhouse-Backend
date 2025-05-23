using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
    [JsonPropertyName("SensorName")]
    public string SensorName { get; set; }
    [Required]
    [JsonPropertyName("Unit")]
    public string Unit { get; set; }
    [Required]
    [JsonPropertyName("Value")]
    public double Value { get; set; }
}
