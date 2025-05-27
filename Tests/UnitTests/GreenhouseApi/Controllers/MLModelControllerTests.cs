using GreenhouseApi.Controllers;
using Domain.DTOs;
using Domain.Entities;
using Domain.IServices;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.UnitTests.GreenhouseApi.Controllers;

public class MlModelControllerTests
{
    private readonly Mock<IMlModelService> _mlModelService = new();
    private readonly MlModelController _controller;

    public MlModelControllerTests()
    {
        _controller = new MlModelController(_mlModelService.Object);
    }

    [Fact]
    public async Task PredictNextWateringTime_ReturnsOk()
    {
        var resultDto = new PredictionResultDto { PredictionTime = DateTime.UtcNow, HoursUntilNextWatering = 5.0 };
        _mlModelService.Setup(s => s.PrepareDataForPredictionAsync(It.IsAny<MlModelDataDto>(), 1)).Returns(Task.CompletedTask);
        _mlModelService.Setup(s => s.PredictNextWateringTimeAsync(It.IsAny<MlModelDataDto>(), 1)).ReturnsAsync(resultDto);

        var result = await _controller.PredictNextWateringTime(1);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetAllPredictionLogs_ReturnsOk()
    {
        _mlModelService.Setup(s => s.GetAllPredictionLogsAsync()).ReturnsAsync(new List<PredictionLog>());

        var result = await _controller.GetAllPredictionLogs();

        Assert.IsType<OkObjectResult>(result.Result);
    }
}