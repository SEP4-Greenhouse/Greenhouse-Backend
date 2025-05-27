using Domain.Entities;

namespace Tests.UnitTests.Domain.Entities;

public class SensorTests
{
    private Greenhouse CreateGreenhouse() => new Greenhouse("GH", "Tomato", new User("User", "u@e.com", "hash"));

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenUnitIsNull()
    {
        var gh = CreateGreenhouse();
        Assert.Throws<ArgumentNullException>(() => new Sensor("Temperature", "Active", null, gh));
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenGreenhouseIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new Sensor("Temperature", "Active", "Celsius", null));
    }

    [Fact]
    public void UpdateStatus_ThrowsArgumentException_WhenStatusIsNullOrWhitespace()
    {
        var sensor = new Sensor("Temperature", "Active", "Celsius", CreateGreenhouse());
        Assert.Throws<ArgumentException>(() => sensor.UpdateStatus(null));
        Assert.Throws<ArgumentException>(() => sensor.UpdateStatus(""));
        Assert.Throws<ArgumentException>(() => sensor.UpdateStatus("   "));
    }

    [Fact]
    public void UpdateStatus_UpdatesStatus_WhenValid()
    {
        var sensor = new Sensor("Temperature", "Active", "Celsius", CreateGreenhouse());
        sensor.UpdateStatus("Inactive");
        Assert.Equal("Inactive", sensor.Status);
    }

    [Fact]
    public void AddReading_AddsReadingToCollection()
    {
        var sensor = new Sensor("Temperature", "Active", "Celsius", CreateGreenhouse());
        var reading = sensor.AddReading(DateTime.UtcNow, 25.5, "Celsius");
        Assert.Contains(reading, sensor.Readings);
    }
}