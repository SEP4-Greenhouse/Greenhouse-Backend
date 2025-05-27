using Domain.Entities;
using Domain.IRepositories;
using Moq;

namespace Tests.UnitTests.GreenhouseService;

public class GreenhouseServiceTests
{
    private readonly Mock<IGreenhouseRepository> _ghRepo = new();
    private readonly Mock<IPlantRepository> _plantRepo = new();
    private readonly Mock<ISensorRepository> _sensorRepo = new();
    private readonly Mock<IActuatorRepository> _actuatorRepo = new();
    private readonly global::GreenhouseService.Services.GreenhouseService _service;

    public GreenhouseServiceTests()
    {
        _service = new global::GreenhouseService.Services.GreenhouseService(_ghRepo.Object, _plantRepo.Object, _sensorRepo.Object, _actuatorRepo.Object);
    }

    [Fact]
    public async Task GetGreenhousesByUserIdAsync_ReturnsGreenhouses()
    {
        _ghRepo.Setup(r => r.GetByUserIdAsync(1)).ReturnsAsync(new List<Greenhouse>());
        var result = await _service.GetGreenhousesByUserIdAsync(1);
        Assert.NotNull(result);
    }
    [Fact]
    public async Task AddPlantToGreenhouseAsync_ThrowsIfGreenhouseNotFound()
    {
        _ghRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Greenhouse?)null);
        var plant = new Plant("Tomato", DateTime.UtcNow, "Seedling", new Greenhouse("GH", "Tomato", 1));
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddPlantToGreenhouseAsync(1, plant));
    }
}