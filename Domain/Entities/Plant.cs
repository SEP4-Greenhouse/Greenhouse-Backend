using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;

public class Plant
{
    [Key]
    public int Id { get; private set; }

    [Required]
    [MaxLength(100)]
    public string Species { get; private set; }

    [Required]
    public DateTime PlantingDate { get; private set; }

    [Required]
    [MaxLength(100)]
    public string GrowthStage { get; private set; }

    [Required]
    public int GreenhouseId { get; private set; }

    [ForeignKey(nameof(GreenhouseId))]
    public Greenhouse Greenhouse { get; private set; }

    public ICollection<SensorReading> AffectingReadings { get; private set; } = new List<SensorReading>();

    public Plant(string species, DateTime plantingDate, string growthStage, Greenhouse greenhouse)
    {
        if (string.IsNullOrWhiteSpace(species)) throw new ArgumentException("Species cannot be empty.");
        if (string.IsNullOrWhiteSpace(growthStage)) throw new ArgumentException("Growth stage cannot be empty.");
        Species = species;
        PlantingDate = plantingDate;
        GrowthStage = growthStage;
        Greenhouse = greenhouse ?? throw new ArgumentNullException(nameof(greenhouse));
    }

    private Plant() { }

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