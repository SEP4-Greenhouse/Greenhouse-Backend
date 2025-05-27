using Domain.Entities;

namespace Tests.Domain.Entities
{
    public class SensorTests
    {
        private class DummyUser : User
        {
            public DummyUser(int id)
                : base("Test User", "test@example.com", "hashedpassword")
            {
                typeof(User)
                    .GetProperty("Id",
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Public)
                    ?.SetValue(this, id);
            }
        }

        private class DummyGreenhouse : Greenhouse
        {
            public DummyGreenhouse(int id)
                : base("TestGreenhouse", "Tomato", new DummyUser(1))
            {
                typeof(Greenhouse)
                    .GetProperty("Id",
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Public)
                    ?.SetValue(this, id);
            }
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenUnitIsNull()
        {
            var greenhouse = new DummyGreenhouse(1);
            Assert.Throws<ArgumentNullException>(() => new Sensor("Temperature", "Active", null, greenhouse));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenGreenhouseIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Sensor("Temperature", "Active", "Celsius", null));
        }

        [Fact]
        public void UpdateStatus_ThrowsArgumentException_WhenStatusIsNullOrWhitespace()
        {
            var sensor = new Sensor("Temperature", "Active", "Celsius", new DummyGreenhouse(1));
            Assert.Throws<ArgumentException>(() => sensor.UpdateStatus(null));
            Assert.Throws<ArgumentException>(() => sensor.UpdateStatus(""));
            Assert.Throws<ArgumentException>(() => sensor.UpdateStatus("   "));
        }

        [Fact]
        public void UpdateStatus_UpdatesStatus_WhenValid()
        {
            var sensor = new Sensor("Temperature", "Active", "Celsius", new DummyGreenhouse(1));
            sensor.UpdateStatus("Inactive");
            Assert.Equal("Inactive", sensor.Status);
        }

        [Fact]
        public void AddReading_AddsReadingToCollection()
        {
            var sensor = new Sensor("Temperature", "Active", "Celsius", new DummyGreenhouse(1));
            var reading = sensor.AddReading(DateTime.UtcNow, 25.5, "Celsius");
            Assert.Contains(reading, sensor.Readings);
        }
    }
}