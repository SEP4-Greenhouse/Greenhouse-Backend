using Domain.Entities;
using Domain.Entities.Actuators;

namespace Tests.UnitTests.Domain.Entities;

public class ActuatorTests
{
    private Greenhouse CreateGreenhouse() => new Greenhouse("GH", "Tomato", new User("User", "u@e.com", "hash"));

    private class TestActuator : Actuator
    {
        public TestActuator(string status, Greenhouse gh) : base(status, gh) { }
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenStatusIsNullOrWhitespace()
    {
        var gh = CreateGreenhouse();
        Assert.Throws<ArgumentException>(() => new TestActuator(null, gh));
        Assert.Throws<ArgumentException>(() => new TestActuator("", gh));
        Assert.Throws<ArgumentException>(() => new TestActuator("   ", gh));
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenGreenhouseIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new TestActuator("Active", null));
    }

    [Fact]
    public void UpdateStatus_ThrowsArgumentException_WhenNullOrWhitespace()
    {
        var actuator = new TestActuator("Active", CreateGreenhouse());
        Assert.Throws<ArgumentException>(() => actuator.UpdateStatus(null));
        Assert.Throws<ArgumentException>(() => actuator.UpdateStatus(""));
        Assert.Throws<ArgumentException>(() => actuator.UpdateStatus("   "));
    }

    [Fact]
    public void UpdateStatus_UpdatesStatus_WhenValid()
    {
        var actuator = new TestActuator("Active", CreateGreenhouse());
        actuator.UpdateStatus("Inactive");
        Assert.Equal("Inactive", actuator.Status);
    }

    [Fact]
    public void InitiateAction_ThrowsArgumentException_WhenTypeIsNullOrWhitespace()
    {
        var actuator = new TestActuator("Active", CreateGreenhouse());
        Assert.Throws<ArgumentException>(() => actuator.InitiateAction(DateTime.Now, null, 1));
        Assert.Throws<ArgumentException>(() => actuator.InitiateAction(DateTime.Now, "", 1));
        Assert.Throws<ArgumentException>(() => actuator.InitiateAction(DateTime.Now, "   ", 1));
    }

    [Fact]
    public void InitiateAction_AddsActionToList()
    {
        var actuator = new TestActuator("Active", CreateGreenhouse());
        var action = actuator.InitiateAction(DateTime.Now, "TestAction", 1);
        Assert.Contains(action, actuator.Actions);
    }
}