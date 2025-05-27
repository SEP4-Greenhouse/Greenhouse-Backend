using Domain.Entities;
using Domain.IRepositories;
using GreenhouseService.Services;
using Moq;

namespace Tests.UnitTests.GreenhouseService;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepo = new();
    private readonly UserService _service;

    public UserServiceTests()
    {
        _service = new UserService(_userRepo.Object);
    }

    [Fact]
    public async Task GetUserByIdAsync_ReturnsUserDto()
    {
        var user = new User("A", "a@b.com", "hash");
        typeof(User).GetProperty("Id")!.SetValue(user, 1); // Set Id via reflection for test
        _userRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
        var result = await _service.GetUserByIdAsync(1);
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetUserByIdAsync_ThrowsIfIdInvalid()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _service.GetUserByIdAsync(0));
    }
}