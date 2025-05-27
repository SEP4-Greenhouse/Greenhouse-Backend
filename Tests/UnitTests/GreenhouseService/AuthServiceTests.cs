using Domain.DTOs;
using Domain.Entities;
using Domain.IRepositories;
using GreenhouseService.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Tests.UnitTests.GreenhouseService;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepo = new();
    private readonly IConfiguration _config;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            { "Jwt:Key", "supersecretkey1234567890" },
            { "Jwt:Issuer", "testissuer" },
            { "Jwt:Audience", "testaudience" },
            { "Jwt:ExpiresInMinutes", "60" }
        };
        _config = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
        _service = new AuthService(_config, _userRepo.Object);
    }

    [Fact]
    public async Task LoginAsync_ReturnsNull_WhenUserNotFound()
    {
        _userRepo.Setup(r => r.GetByEmailAsync("a@b.com")).ReturnsAsync((User?)null);
        var result = await _service.LoginAsync(new LoginRequestDto("a@b.com", "pw"));
        Assert.Null(result);
    }

    [Fact]
    public async Task RegisterAsync_Throws_WhenUserExists()
    {
        _userRepo.Setup(r => r.ExistsByEmailAsync("a@b.com")).ReturnsAsync(true);
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.RegisterAsync(new CreateUserDto("A", "a@b.com", "pw")));
    }

    [Fact]
    public async Task RegisterAsync_ReturnsUserDto_WhenValid()
    {
        _userRepo.Setup(r => r.ExistsByEmailAsync("a@b.com")).ReturnsAsync(false);
        _userRepo.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync(new User("A", "a@b.com", "hash"));
        var result = await _service.RegisterAsync(new CreateUserDto("A", "a@b.com", "pw"));
        Assert.NotNull(result);
        Assert.Equal("A", result.Name);
    }
    [Fact]
    public void VerifyPassword_ReturnsFalse_WhenInvalid()
    {
        var validHash = BCrypt.Net.BCrypt.HashPassword("otherpassword");
        var result = _service.VerifyPassword("plain", validHash);
        Assert.False(result);
    }
}