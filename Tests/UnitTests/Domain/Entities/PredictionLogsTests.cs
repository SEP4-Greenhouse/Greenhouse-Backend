using Domain.Entities;

namespace Tests.UnitTests.Domain.Entities;

public class PredictionLogTests
{
    [Fact]
    public void Can_Set_And_Get_Properties()
    {
        var plant = new Plant("Species", DateTime.UtcNow, "Stage", new Greenhouse("GH", "Tomato", new User("User", "u@e.com", "hash")));
        var log = new PredictionLog
        {
            PredictionTime = DateTime.UtcNow,
            HoursUntilNextWatering = 12.5,
            Plant = plant
        };
        Assert.Equal(12.5, log.HoursUntilNextWatering);
        Assert.Equal(plant, log.Plant);
    }
}