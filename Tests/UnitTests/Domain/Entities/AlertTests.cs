using Domain.Entities;

namespace Tests.UnitTests.Domain.Entities;

public class AlertTests
{
    [Fact]
    public void Constructor_ThrowsArgumentException_WhenMessageIsNullOrWhitespace()
    {
        Assert.Throws<ArgumentException>(() => new Alert(Alert.AlertType.Sensor, null));
        Assert.Throws<ArgumentException>(() => new Alert(Alert.AlertType.Sensor, ""));
        Assert.Throws<ArgumentException>(() => new Alert(Alert.AlertType.Sensor, "   "));
    }

    [Fact]
    public void UpdateMessage_ThrowsArgumentException_WhenNullOrWhitespace()
    {
        var alert = new Alert(Alert.AlertType.Sensor, "msg");
        Assert.Throws<ArgumentException>(() => alert.UpdateMessage(null));
        Assert.Throws<ArgumentException>(() => alert.UpdateMessage(""));
        Assert.Throws<ArgumentException>(() => alert.UpdateMessage("   "));
    }

    [Fact]
    public void UpdateMessage_UpdatesMessage_WhenValid()
    {
        var alert = new Alert(Alert.AlertType.Sensor, "msg");
        alert.UpdateMessage("new");
        Assert.Equal("new", alert.Message);
    }

    [Fact]
    public void AddTriggeringSensorReading_ThrowsArgumentNullException_WhenNull()
    {
        var alert = new Alert(Alert.AlertType.Sensor, "msg");
        Assert.Throws<ArgumentNullException>(() => alert.AddTriggeringSensorReading(null));
    }

    [Fact]
    public void AddTriggeringActuatorAction_ThrowsArgumentNullException_WhenNull()
    {
        var alert = new Alert(Alert.AlertType.Sensor, "msg");
        Assert.Throws<ArgumentNullException>(() => alert.AddTriggeringActuatorAction(null));
    }
}