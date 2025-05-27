using GreenhouseApi.Controllers;
using Domain.Entities;
using Domain.IServices;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace Tests.UnitTests.GreenhouseApi.Controllers;

public class AlertsControllerTests
{
    private readonly Mock<IAlertService> _alertService = new();
    private readonly AlertsController _controller;

    public AlertsControllerTests()
    {
        _controller = new AlertsController(_alertService.Object);
    }

    [Fact]
    public async Task GetAlertsByType_ReturnsOk()
    {
        _alertService.Setup(s => s.GetAlertsByTypeAsync(Alert.AlertType.Sensor)).ReturnsAsync(new List<Alert>());

        var result = await _controller.GetAlertsByType(Alert.AlertType.Sensor);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetAlertsByDateRange_ReturnsOk()
    {
        _alertService.Setup(s => s.GetAlertsByDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new List<Alert>());

        var result = await _controller.GetAlertsByDateRange(DateTime.UtcNow, DateTime.UtcNow);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetAllAlerts_ReturnsOk()
    {
        _alertService.Setup(s => s.GetAllAlertsAsync()).ReturnsAsync(new List<Alert>());

        var result = await _controller.GetAllAlerts();

        Assert.IsType<OkObjectResult>(result);
    }
}