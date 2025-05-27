using GreenhouseApi.Controllers;
using Domain.DTOs;
using Domain.Entities;
using Domain.IServices;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.UnitTests.GreenhouseApi.Controllers;

public class SensorControllerTests
{
    private readonly Mock<ISensorService> _sensorService = new();
    private readonly SensorController _controller;

    public SensorControllerTests()
    {
        _controller = new SensorController(_sensorService.Object);
    }

    [Fact]
    public async Task AddSensorReading_ReturnsOk()
    {
        var sensor = new Sensor("Temperature", "Active", "Celsius", new Greenhouse("GH", "Tomato", 1));
        _sensorService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(sensor);
        _sensorService.Setup(s => s.AddSensorReadingAsync(It.IsAny<SensorReading>())).Returns(Task.CompletedTask);

        var readingDto = new SensorReadingDto { TimeStamp = DateTime.UtcNow, Value = 25.0 };
        var result = await _controller.AddSensorReading(readingDto, 1);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetLatestReadingFromAllSensors_ReturnsOk()
    {
        _sensorService.Setup(s => s.GetLatestReadingFromAllSensorsAsync()).ReturnsAsync(new List<SensorReading>());

        var result = await _controller.GetLatestReadingFromAllSensors();

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetReadingsBySensor_ReturnsOk()
    {
        _sensorService
            .Setup(s => s.GetReadingsBySensorAsync())
            .ReturnsAsync(new Dictionary<int, IEnumerable<SensorReading>>());

        var result = await _controller.GetReadingsBySensor();

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetLatestReadingBySensor_ReturnsOk_WhenFound()
    {
        var reading = new SensorReading(DateTime.UtcNow, 10, "Celsius", new Sensor("Temperature", "Active", "Celsius", new Greenhouse("GH", "Tomato", 1)));
        _sensorService.Setup(s => s.GetLatestReadingBySensorAsync()).ReturnsAsync(new Dictionary<int, SensorReading> { { 1, reading } });

        var result = await _controller.GetLatestReadingBySensor(1);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetLatestReadingBySensor_ReturnsNotFound_WhenMissing()
    {
        _sensorService.Setup(s => s.GetLatestReadingBySensorAsync()).ReturnsAsync(new Dictionary<int, SensorReading>());

        var result = await _controller.GetLatestReadingBySensor(1);

        Assert.IsType<NotFoundObjectResult>(result);
    }
}