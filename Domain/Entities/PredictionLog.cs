using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class PredictionLog
{
    [Key] public int Id { get; set; }
    [Required] public DateTime Timestamp { get; set; }
    [Required] [MaxLength(100)] public string Status { get; set; }
    [Required] public string Suggestion { get; set; }
    [Required] public string TrendAnalysis { get; set; }
}