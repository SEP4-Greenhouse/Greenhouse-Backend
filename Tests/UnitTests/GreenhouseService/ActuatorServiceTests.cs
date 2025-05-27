using Domain.Entities;
using Domain.Entities.Actuators;
using Domain.IRepositories;
using Domain.IServices;
using GreenhouseService.Services;
using Moq;

namespace Tests.UnitTests.GreenhouseService;

public class ActuatorServiceTests
{
    private readonly Mock<IActuatorRepository> _actuatorRepo = new();
    private readonly Mock<IAlertService> _alertService = new();
    private readonly ActuatorService _service;

    public ActuatorServiceTests()
    {
        _service = new ActuatorService(_actuatorRepo.Object, _alertService.Object);
    }

    [Fact]
    public async Task TriggerActuatorActionAsync_Valid_CreatesActionAndAlert()
    {
        var user = new User("User", "u@e.com", "hash");
        var greenhouse = new Greenhouse("GH", "Tomato", user);
        var actuator = new WaterPumpActuator("Active", greenhouse);

        _actuatorRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(actuator);

        var result = await _service.TriggerActuatorActionAsync(1, "TurnOn", 2.0);

        Assert.NotNull(result);
        _alertService.Verify(a => a.CreateActuatorAlertAsync(result, It.IsAny<string>()), Times.Once);
        _actuatorRepo.Verify(r => r.UpdateAsync(actuator), Times.Once);
    }

    [Fact]
    public async Task TriggerActuatorActionAsync_InvalidId_Throws()
    {
        _actuatorRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Actuator?)null);
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.TriggerActuatorActionAsync(1, "TurnOn", 2.0));
    }
}