using Domain.Entities;


namespace Tests.Domain.Entities;

public class GreenhouseTests
{
    private User CreateUser() => new User("Test User", "test@example.com", "hashedpassword");

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenNameIsNullOrWhitespace()
    {
        var user = CreateUser();
        Assert.Throws<ArgumentException>(() => new Greenhouse(null, "Tomato", user));
        Assert.Throws<ArgumentException>(() => new Greenhouse("", "Tomato", user));
        Assert.Throws<ArgumentException>(() => new Greenhouse("   ", "Tomato", user));
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenPlantTypeIsNullOrWhitespace()
    {
        var user = CreateUser();
        Assert.Throws<ArgumentException>(() => new Greenhouse("Greenhouse", null, user));
        Assert.Throws<ArgumentException>(() => new Greenhouse("Greenhouse", "", user));
        Assert.Throws<ArgumentException>(() => new Greenhouse("Greenhouse", "   ", user));
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenUserIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new Greenhouse("Greenhouse", "Tomato", (User)null));
    }

    [Fact]
    public void UpdatePlantType_ThrowsArgumentException_WhenNullOrWhitespace()
    {
        var gh = new Greenhouse("GH", "Tomato", CreateUser());
        Assert.Throws<ArgumentException>(() => gh.UpdatePlantType(null));
        Assert.Throws<ArgumentException>(() => gh.UpdatePlantType(""));
        Assert.Throws<ArgumentException>(() => gh.UpdatePlantType("   "));
    }

    [Fact]
    public void UpdatePlantType_UpdatesPlantType_WhenValid()
    {
        var gh = new Greenhouse("GH", "Tomato", CreateUser());
        gh.UpdatePlantType("Cucumber");
        Assert.Equal("Cucumber", gh.PlantType);
    }

    [Fact]
    public void UpdateName_ThrowsArgumentException_WhenNullOrWhitespace()
    {
        var gh = new Greenhouse("GH", "Tomato", CreateUser());
        Assert.Throws<ArgumentException>(() => gh.UpdateName(null));
        Assert.Throws<ArgumentException>(() => gh.UpdateName(""));
        Assert.Throws<ArgumentException>(() => gh.UpdateName("   "));
    }

    [Fact]
    public void UpdateName_UpdatesName_WhenValid()
    {
        var gh = new Greenhouse("GH", "Tomato", CreateUser());
        gh.UpdateName("NewName");
        Assert.Equal("NewName", gh.Name);
    }

    [Fact]
    public void AddPlant_AddsPlantToCollection()
    {
        var gh = new Greenhouse("GH", "Tomato", CreateUser());
        var plant = new Plant("Species", DateTime.UtcNow, "Seedling", gh);
        gh.AddPlant(plant);
        Assert.Contains(plant, gh.Plants);
    }
}