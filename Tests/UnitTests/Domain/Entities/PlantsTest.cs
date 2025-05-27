using Domain.Entities;

namespace Tests.UnitTests.Domain.Entities;

public class PlantTests
{
    private Greenhouse CreateGreenhouse() => new Greenhouse("GH", "Tomato", new User("User", "u@e.com", "hash"));

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenSpeciesOrGrowthStageIsNullOrWhitespace()
    {
        var gh = CreateGreenhouse();
        Assert.Throws<ArgumentException>(() => new Plant(null, DateTime.UtcNow, "Stage", gh));
        Assert.Throws<ArgumentException>(() => new Plant("", DateTime.UtcNow, "Stage", gh));
        Assert.Throws<ArgumentException>(() => new Plant("Species", DateTime.UtcNow, null, gh));
        Assert.Throws<ArgumentException>(() => new Plant("Species", DateTime.UtcNow, "", gh));
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenGreenhouseIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new Plant("Species", DateTime.UtcNow, "Stage", null));
    }

    [Fact]
    public void UpdateGrowthStage_ThrowsArgumentException_WhenNullOrWhitespace()
    {
        var plant = new Plant("Species", DateTime.UtcNow, "Stage", CreateGreenhouse());
        Assert.Throws<ArgumentException>(() => plant.UpdateGrowthStage(null));
        Assert.Throws<ArgumentException>(() => plant.UpdateGrowthStage(""));
        Assert.Throws<ArgumentException>(() => plant.UpdateGrowthStage("   "));
    }

    [Fact]
    public void UpdateGrowthStage_UpdatesGrowthStage_WhenValid()
    {
        var plant = new Plant("Species", DateTime.UtcNow, "Stage", CreateGreenhouse());
        plant.UpdateGrowthStage("Flowering");
        Assert.Equal("Flowering", plant.GrowthStage);
    }

    [Fact]
    public void UpdateSpecies_ThrowsArgumentException_WhenNullOrWhitespace()
    {
        var plant = new Plant("Species", DateTime.UtcNow, "Stage", CreateGreenhouse());
        Assert.Throws<ArgumentException>(() => plant.UpdateSpecies(null));
        Assert.Throws<ArgumentException>(() => plant.UpdateSpecies(""));
        Assert.Throws<ArgumentException>(() => plant.UpdateSpecies("   "));
    }

    [Fact]
    public void UpdateSpecies_UpdatesSpecies_WhenValid()
    {
        var plant = new Plant("Species", DateTime.UtcNow, "Stage", CreateGreenhouse());
        plant.UpdateSpecies("NewSpecies");
        Assert.Equal("NewSpecies", plant.Species);
    }
}