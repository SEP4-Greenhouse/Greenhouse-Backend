using Domain.Entities;
using Domain.Entities.Actuators;

namespace Tests.Domain.Entities;

public class ActuatorActionTests
{
    private Greenhouse CreateGreenhouse() => new Greenhouse("GH", "Tomato", new User("User", "u@e.com", "hash"));
    private Actuator CreateActuator() => new WaterPumpActuator("Active", CreateGreenhouse());

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenActionIsNullOrWhitespace()
    {
        var actuator = CreateActuator();
        Assert.Throws<ArgumentException>(() => new ActuatorAction(DateTime.Now, null, 1, actuator));
        Assert.Throws<ArgumentException>(() => new ActuatorAction(DateTime.Now, "", 1, actuator));
        Assert.Throws<ArgumentException>(() => new ActuatorAction(DateTime.Now, "   ", 1, actuator));
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenActuatorIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ActuatorAction(DateTime.Now, "Action", 1, null));
    }

    [Fact]
    public void TriggerAlert_ThrowsArgumentException_WhenMessageIsNullOrWhitespace()
    {
        var actuator = CreateActuator();
        var action = new ActuatorAction(DateTime.Now, "Action", 1, actuator);
        Assert.Throws<ArgumentException>(() => action.TriggerAlert(Alert.AlertType.Actuator, null));
        Assert.Throws<ArgumentException>(() => action.TriggerAlert(Alert.AlertType.Actuator, ""));
        Assert.Throws<ArgumentException>(() => action.TriggerAlert(Alert.AlertType.Actuator, "   "));
    }

    [Fact]
    public void TriggerAlert_AddsAlertToTriggeredAlerts()
    {
        var actuator = CreateActuator();
        var action = new ActuatorAction(DateTime.Now, "Action", 1, actuator);
        var alert = action.TriggerAlert(Alert.AlertType.Actuator, "Alert message");
        Assert.Contains(alert, action.TriggeredAlerts);
    }
}