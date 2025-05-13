using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

public class PredictionResultDto
{
    [Required(ErrorMessage = "Timestamp is required.")]
    public DateTime Timestamp { get; init; }

    [Required(ErrorMessage = "Status is required.")]
    public string Status { get; init; }

    [Required(ErrorMessage = "Suggestion is required.")]
    public string Suggestion { get; init; }
}