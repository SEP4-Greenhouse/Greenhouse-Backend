namespace Domain.DTOs;

public class PredictionResultDto
{
    public DateTime Timestamp { get; set; }
    public string Status { get; set; }         // e.g., "warning", "normal"
    public string Suggestion { get; set; }     // e.g., "Activate water pump"
}