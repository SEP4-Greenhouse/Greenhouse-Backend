using Domain.Entities;
using Domain.IRepositories;
using GreenhouseService.Services;
using Moq;

namespace Tests.UnitTests.GreenhouseService;

public class AlertServiceTests
{
    private readonly Mock<IAlertRepository> _alertRepo = new();
    private readonly AlertService _service;

    public AlertServiceTests()
    {
        _service = new AlertService(_alertRepo.Object);
    }

    [Fact]
    public async Task GetAlertsByTypeAsync_ReturnsAlerts()
    {
        _alertRepo.Setup(r => r.GetByTypeAsync(Alert.AlertType.Sensor)).ReturnsAsync(new List<Alert>());
        var result = await _service.GetAlertsByTypeAsync(Alert.AlertType.Sensor);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateSensorAlertAsync_AddsAlert()
    {
        var user = new User("TestUser", "test@example.com", "hash");
        var greenhouse = new Greenhouse("GH", "Tomato", user);
        var sensor = new Sensor("Temperature", "Active", "C", greenhouse);
        var reading = new SensorReading(DateTime.UtcNow, 25.0, "C", sensor);

        _alertRepo.Setup(r => r.AddAsync(It.IsAny<Alert>())).ReturnsAsync(new Alert(Alert.AlertType.Sensor, "msg"));
        await _service.CreateSensorAlertAsync(reading, "msg");
        _alertRepo.Verify(r => r.AddAsync(It.IsAny<Alert>()), Times.Once);
    }
}