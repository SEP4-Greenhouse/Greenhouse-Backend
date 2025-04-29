namespace Domain.DTOs;

public class PredictionResultDto
{
    public DateTime Timestamp { get; init; }
    public string Status { get; init; }
    public string Suggestion { get; init; }

    public PredictionResultDto(DateTime timestamp, string status, string suggestion)
    {
        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("Status cannot be empty.");
        if (string.IsNullOrWhiteSpace(suggestion))
            throw new ArgumentException("Suggestion cannot be empty.");

        Timestamp = timestamp;
        Status = status;
        Suggestion = suggestion;
    }
}