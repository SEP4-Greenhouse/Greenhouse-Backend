using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class PredictionLog
{
    [Key] public int Id { get; set; }

    [Required] public DateTime Timestamp { get; set; }

    [Required] [MaxLength(50)] public string SensorType { get; set; } = null!;

    [Required] public float Value { get; set; }

    [Required] [MaxLength(100)] public string Status { get; set; } = null!;

    [Required] public string Suggestion { get; set; } = null!;

    public string? TrendAnalysis { get; set; }  // ✅ optional
}