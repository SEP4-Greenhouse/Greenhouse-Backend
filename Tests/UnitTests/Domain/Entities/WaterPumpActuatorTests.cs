using Domain.Entities;

namespace Tests.UnitTests.Domain.Entities;

public class WaterPumpActuatorTests
{
    private Greenhouse CreateGreenhouse() => new Greenhouse("GH", "Tomato", new User("User", "u@e.com", "hash"));

    [Fact]
    public void SetFlowRate_ThrowsArgumentException_WhenFlowRateIsZeroOrNegative()
    {
        var actuator = new WaterPumpActuator("Active", CreateGreenhouse());
        Assert.Throws<ArgumentException>(() => actuator.SetFlowRate(0));
        Assert.Throws<ArgumentException>(() => actuator.SetFlowRate(-1));
    }

    [Fact]
    public void SetFlowRate_ReturnsAction_WhenValid()
    {
        var actuator = new WaterPumpActuator("Active", CreateGreenhouse());
        var action = actuator.SetFlowRate(2.5);
        Assert.Equal("SetFlowRate", action.Action);
        Assert.Equal(2.5, action.Value);
    }
}