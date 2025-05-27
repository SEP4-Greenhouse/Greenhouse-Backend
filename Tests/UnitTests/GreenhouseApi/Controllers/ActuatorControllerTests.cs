using GreenhouseApi.Controllers;
using Domain.DTOs;
using Domain.Entities;
using Domain.IServices;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.UnitTests.GreenhouseApi.Controllers;

public class ActuatorControllerTests
{
    private readonly Mock<IActuatorService> _actuatorService = new();
    private readonly ActuatorController _controller;

    public ActuatorControllerTests()
    {
        _controller = new ActuatorController(_actuatorService.Object);
    }

    [Fact]
    public async Task TriggerActuatorAction_ReturnsOk()
    {
        var actionDto = new ActuatorActionDto(1.0, "TurnOn") { Timestamp = DateTime.UtcNow };
        _actuatorService
            .Setup(s => s.TriggerActuatorActionAsync(1, It.IsAny<string>(), It.IsAny<double>()))
            .ReturnsAsync((ActuatorAction?)null);

        var result = await _controller.TriggerActuatorAction(1, actionDto);

        Assert.IsType<OkResult>(result);
    }
}