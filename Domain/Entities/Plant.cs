using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Plant
{
    [Key] public int Id { get; private set; }
    [Required] [MaxLength(100)] public string Species { get; private set; }
    [Required] public DateTime PlantingDate { get; private set; }
    [Required] [MaxLength(100)] public string GrowthStage { get; private set; }
    [ForeignKey("Greenhouse")] public int GreenhouseId { get; private set; }
    public Greenhouse Greenhouse { get; private set; }

    public ICollection<SensorReading> AffectingReadings { get; private set; } = new List<SensorReading>();

    public Plant(string species, DateTime plantingDate, string growthStage, Greenhouse greenhouse)
    {
        Species = species;
        PlantingDate = plantingDate;
        GrowthStage = growthStage;
        Greenhouse = greenhouse;
        GreenhouseId = greenhouse.Id;
    }

    private Plant()
    {
    } // Required by EF Core

    public void UpdateGrowthStage(string newGrowthStage)
    {
        if (string.IsNullOrWhiteSpace(newGrowthStage))
            throw new ArgumentException("Growth stage cannot be empty.");
        GrowthStage = newGrowthStage;
    }

    public void UpdateSpecies(string newSpecies)
    {
        if (string.IsNullOrWhiteSpace(newSpecies))
            throw new ArgumentException("Species cannot be empty.");
        Species = newSpecies;
    }
}