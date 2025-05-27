using Domain.IRepositories;
using GreenhouseService.Services;
using Moq;

namespace Tests.UnitTests.GreenhouseService;

public class DummyEntity { }

public class BaseServiceTests
{
    private readonly Mock<IBaseRepository<DummyEntity>> _repo = new();
    private readonly BaseService<DummyEntity> _service;

    public BaseServiceTests()
    {
        _service = new BaseService<DummyEntity>(_repo.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEntities()
    {
        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<DummyEntity>());
        var result = await _service.GetAllAsync();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task AddAsync_AddsEntity()
    {
        var entity = new DummyEntity();
        _repo.Setup(r => r.AddAsync(entity)).ReturnsAsync(entity);
        var result = await _service.AddAsync(entity);
        Assert.Equal(entity, result);
    }
}