namespace Domain.DTOs;

public abstract class PredictionResultDto
{
    public DateTime PredictionTime { get; set; }
    public double HoursUntilNextWatering { get; set; }
}