using GreenhouseApi.Controllers;
using Domain.DTOs;
using Domain.Entities;
using Domain.Entities.Actuators;
using Domain.IServices;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.UnitTests.GreenhouseApi.Controllers;

public class GreenhouseControllerTests
{
    private readonly Mock<IGreenhouseService> _greenhouseService = new();
    private readonly Mock<IUserService> _userService = new();
    private readonly GreenhouseController _controller;

    public GreenhouseControllerTests()
    {
        _controller = new GreenhouseController(_greenhouseService.Object, _userService.Object);
    }

    [Fact]
    public async Task GetGreenhousesByUserId_ReturnsOk()
    {
        _greenhouseService.Setup(s => s.GetGreenhousesByUserIdAsync(1)).ReturnsAsync(new List<Greenhouse> { new Greenhouse("GH", "Tomato", 1) });

        var result = await _controller.GetGreenhousesByUserId(1);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task CreateGreenhouse_ReturnsOk_WhenValid()
    {
        var dto = new GreenhouseDto("GH", "Tomato", 1);
        _userService.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(new UserDto(1, "A", "a@b.com"));
        _greenhouseService
            .Setup(s => s.AddAsync(It.IsAny<Greenhouse>()))
            .ReturnsAsync(new Greenhouse("GH", "Tomato", 1));

        var result = await _controller.CreateGreenhouse(dto);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task CreateGreenhouse_ReturnsBadRequest_WhenInvalid()
    {
        var dto = new GreenhouseDto(null, null, 1);
        _userService.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync((UserDto?)null);

        var result = await _controller.CreateGreenhouse(dto);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    // GreenhouseControllerTests.cs
    [Fact]
    public async Task UpdateGreenhouseName_ReturnsOk()
    {
        var gh = new Greenhouse("GH", "Tomato", 1);
        _greenhouseService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(gh);
        _greenhouseService.Setup(s => s.UpdateAsync(gh)).Returns(Task.CompletedTask);

        var result = await _controller.UpdateGreenhouseName(1, "NewName");

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task DeleteGreenhouse_ReturnsOk()
    {
        _greenhouseService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

        var result = await _controller.DeleteGreenhouse(1);

        Assert.IsType<OkObjectResult>(result);
    }
}