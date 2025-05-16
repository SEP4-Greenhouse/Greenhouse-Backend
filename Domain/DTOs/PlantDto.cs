namespace Domain.DTOs;

public class PlantDto
{
    public string Species { get; private set; }
    
    public DateTime PlantingDate { get; private set; }
    
    public string GrowthStage { get; private set; }
}