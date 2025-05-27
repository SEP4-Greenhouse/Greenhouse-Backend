using GreenhouseApi.Controllers;
using Domain.DTOs;
using Domain.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace Tests.UnitTests.GreenhouseApi.Controllers;

public class UserControllerTests
{
    private readonly Mock<IUserService> _userService = new();
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _controller = new UserController(_userService.Object);
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1")
        }));
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task GetMyUserInfo_ReturnsOk_WhenUserExists()
    {
        var userDto = new UserDto(1, "Test", "test@email.com");
        _userService.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(userDto);

        var result = await _controller.GetMyUserInfo();

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetMyUserInfo_ReturnsNotFound_WhenUserMissing()
    {
        _userService.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync((UserDto?)null);

        var result = await _controller.GetMyUserInfo();

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeleteUser_ReturnsNoContent()
    {
        var result = await _controller.DeleteUser();
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateUserName_ReturnsNoContent()
    {
        var result = await _controller.UpdateUserName("newName");
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdatePassword_ReturnsNoContent()
    {
        var result = await _controller.UpdatePassword("pw");
        Assert.IsType<NoContentResult>(result);
    }
}