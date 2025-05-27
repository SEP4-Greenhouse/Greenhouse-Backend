using Domain.Entities;

namespace Tests.UnitTests.Domain.Entities;

public class ThresholdTests
{
    [Fact]
    public void Can_Set_And_Get_Properties()
    {
        var sensor = new Sensor("Temperature", "Active", "Celsius", new Greenhouse("GH", "Tomato", new User("User", "u@e.com", "hash")));
        var threshold = new Threshold
        {
            MinValue = 10,
            MaxValue = 30,
            Sensor = sensor
        };
        Assert.Equal(10, threshold.MinValue);
        Assert.Equal(30, threshold.MaxValue);
        Assert.Equal(sensor, threshold.Sensor);
    }
}