using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Threshold
{
    [Key]
    public int Id { get; init; }
    public double MinValue { get; set; }
    public double MaxValue { get; set; }

    [ForeignKey("Sensor")]
    public int SensorId { get; init; }
    public Sensor Sensor { get; init; }
}