namespace Domain.DTOs;

public class MlModelDataDto
{
    public MlModelDataDto(DateTime timestamp, string plantGrowthStage, double timeSinceLastWateringInHours, List<MlSensorReadingDto> mlSensorReadings)
    {
        Timestamp = timestamp;
        PlantGrowthStage = plantGrowthStage;
        TimeSinceLastWateringInHours = timeSinceLastWateringInHours;
        MlSensorReadings = mlSensorReadings;
    }
    public MlModelDataDto()
    {
    }

    public DateTime Timestamp { get; set; }
    public String PlantGrowthStage { get; set; }
    public double TimeSinceLastWateringInHours { get; set; }
    public List<MlSensorReadingDto> MlSensorReadings { get; set; }
}