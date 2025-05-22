namespace Domain.DTOs;

public class PlantDto(string species, DateTime plantingDate, string growthStage)
{
    public string Species { get; set; } = species;

    public DateTime PlantingDate { get; set; } = plantingDate;

    public string GrowthStage { get; set; } = growthStage;
}