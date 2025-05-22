using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class PredictionLog
{
    [Key] public int Id { get; set; }

    [Required] public DateTime PredictionTime { get; set; }

    [Required] public double HoursUntilNextWatering { get; set; }

    [Required]
    [ForeignKey("Plant")]
    public int PlantId { get; set; }
    public Plant Plant { get; set; } = null!;
}