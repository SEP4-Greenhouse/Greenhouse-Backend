namespace Domain.DTOs;

public class MlModelDataDto
{
    public DateTime Timestamp { get; set; }
    public String PlantGrowthStage { get; set; }
    public double TimeSinceLastWateringInHours { get; set; }
    public List<MlSensorReadingDto> MlSensorReadings { get; set; }
}