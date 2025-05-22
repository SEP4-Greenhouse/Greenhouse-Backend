using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class PredictionLog
{
    [Key] public int Id { get; init; }

    [Required] public DateTime PredictionTime { get; init; }

    [Required] public double HoursUntilNextWatering { get; init; }

    [Required] [ForeignKey("Plant")] public int PlantId { get; init; }
    public Plant Plant { get; init; } = null!;
}