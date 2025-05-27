using GreenhouseApi.Controllers;
using Domain.DTOs;
using Domain.IServices;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.UnitTests.GreenhouseApi.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authService = new();
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _controller = new AuthController(_authService.Object);
    }

    [Fact]
    public async Task Login_ReturnsOk_WhenCredentialsValid()
    {
        var loginRequest = new LoginRequestDto("user@email.com", "password");
        var response = new LoginResponseDto { Token = "token", Expiry = DateTime.UtcNow.AddHours(1) };
        _authService.Setup(s => s.LoginAsync(loginRequest)).ReturnsAsync(response);

        var result = await _controller.Login(loginRequest);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenCredentialsInvalid()
    {
        var loginRequest = new LoginRequestDto("user@email.com", "wrongpw");
        _authService.Setup(s => s.LoginAsync(loginRequest)).ReturnsAsync((LoginResponseDto?)null);

        var result = await _controller.Login(loginRequest);

        Assert.IsType<UnauthorizedResult>(result.Result);
    }

    [Fact]
    public async Task Register_ReturnsCreated_WhenValid()
    {
        var createUserDto = new CreateUserDto("A", "a@b.com", "pw");
        var userDto = new UserDto(1, "A", "a@b.com");
        _authService.Setup(s => s.RegisterAsync(createUserDto)).ReturnsAsync(userDto);

        var result = await _controller.Register(createUserDto);

        Assert.IsType<CreatedAtActionResult>(result.Result);
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_WhenMissingFields()
    {
        var createUserDto = new CreateUserDto("", "", "");

        var result = await _controller.Register(createUserDto);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public void HealthCheck_ReturnsOk()
    {
        var result = _controller.HealthCheck();
        Assert.IsType<OkObjectResult>(result);
    }
}