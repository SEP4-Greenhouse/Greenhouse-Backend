using Domain.Entities;

namespace Tests.UnitTests.Domain.Entities;

public class SensorReadingTests
{
    private Sensor CreateSensor() => new Sensor("Temperature", "Active", "Celsius", new Greenhouse("GH", "Tomato", new User("User", "u@e.com", "hash")));

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenUnitIsNullOrWhitespace()
    {
        var sensor = CreateSensor();
        Assert.Throws<ArgumentException>(() => new SensorReading(DateTime.UtcNow, 10, null, sensor));
        Assert.Throws<ArgumentException>(() => new SensorReading(DateTime.UtcNow, 10, "", sensor));
        Assert.Throws<ArgumentException>(() => new SensorReading(DateTime.UtcNow, 10, "   ", sensor));
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenSensorIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new SensorReading(DateTime.UtcNow, 10, "Celsius", null));
    }

    [Fact]
    public void AddAffectedPlant_AddsPlantToCollection()
    {
        var reading = new SensorReading(DateTime.UtcNow, 10, "Celsius", CreateSensor());
        var plant = new Plant("Species", DateTime.UtcNow, "Stage", new Greenhouse("GH", "Tomato", new User("User", "u@e.com", "hash")));
        reading.AddAffectedPlant(plant);
        Assert.Contains(plant, reading.AffectedPlants);
    }

    [Fact]
    public void TriggerAlert_ThrowsArgumentException_WhenMessageIsNullOrWhitespace()
    {
        var reading = new SensorReading(DateTime.UtcNow, 10, "Celsius", CreateSensor());
        Assert.Throws<ArgumentException>(() => reading.TriggerAlert(Alert.AlertType.Sensor, null));
        Assert.Throws<ArgumentException>(() => reading.TriggerAlert(Alert.AlertType.Sensor, ""));
        Assert.Throws<ArgumentException>(() => reading.TriggerAlert(Alert.AlertType.Sensor, "   "));
    }

    [Fact]
    public void TriggerAlert_AddsAlertToTriggeredAlerts()
    {
        var reading = new SensorReading(DateTime.UtcNow, 10, "Celsius", CreateSensor());
        var alert = reading.TriggerAlert(Alert.AlertType.Sensor, "Alert message");
        Assert.Contains(alert, reading.TriggeredAlerts);
    }
}