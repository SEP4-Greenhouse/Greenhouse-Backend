using System;
using System.ComponentModel.DataAnnotations;

public class PredictionLog
{
    [Key]
    public int Id { get; set; }  // Primary key

    public DateTime Timestamp { get; set; }  // Use DateTime for a timestamp field
    public string Status { get; set; }  // Example: "Success", "Failure"
    public string Suggestion { get; set; }  // Example: "Increase watering", "Decrease light"
    public string TrendAnalysis { get; set; }  // Example: "Stable", "Increasing"
}