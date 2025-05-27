using Domain.DTOs;
using Domain.Entities;
using Domain.IClients;
using Domain.IRepositories;
using GreenhouseService.Services;
using Moq;

namespace Tests.UnitTests.GreenhouseService;

public class MlModelServiceTests
{
    private readonly Mock<IMlHttpClient> _mlClient = new();
    private readonly Mock<ISensorReadingRepository> _readingRepo = new();
    private readonly Mock<IPlantRepository> _plantRepo = new();
    private readonly Mock<IPredictionLogRepository> _logRepo = new();
    private readonly MlModelService _service;

    public MlModelServiceTests()
    {
        _service = new MlModelService(_mlClient.Object, _readingRepo.Object, _plantRepo.Object, _logRepo.Object);
    }

    [Fact]
    public async Task PredictNextWateringTimeAsync_LogsPrediction()
    {
        var prediction = new PredictionResultDto { PredictionTime = DateTime.UtcNow, HoursUntilNextWatering = 5 };
        _mlClient.Setup(c => c.PredictNextWateringTimeAsync(It.IsAny<MlModelDataDto>())).ReturnsAsync(prediction);
        _logRepo.Setup(r => r.AddAsync(It.IsAny<PredictionLog>())).ReturnsAsync(new PredictionLog());

        var result = await _service.PredictNextWateringTimeAsync(new MlModelDataDto(), 1);

        Assert.Equal(prediction.HoursUntilNextWatering, result.HoursUntilNextWatering);
        _logRepo.Verify(r => r.AddAsync(It.IsAny<PredictionLog>()), Times.Once);
    }

    [Fact]
    public async Task PrepareDataForPredictionAsync_Throws_WhenPlantNotFound()
    {
        _plantRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Plant?)null);
        await Assert.ThrowsAsync<Exception>(() => _service.PrepareDataForPredictionAsync(new MlModelDataDto(), 1));
    }

    [Fact]
    public async Task AddPredictionLogAsync_Throws_WhenLogIsNull()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.AddPredictionLogAsync(null));
    }
}