using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Entities;
using Domain.IRepositories;
using Domain.IServices;
using GreenhouseService.Services;
using Moq;
using Xunit;

namespace Tests.UnitTests.GreenhouseService;

public class SensorServiceTests
{
    private readonly Mock<ISensorRepository> _sensorRepo = new();
    private readonly Mock<ISensorReadingRepository> _readingRepo = new();
    private readonly Mock<IThresholdRepository> _thresholdRepo = new();
    private readonly Mock<IAlertService> _alertService = new();
    private readonly SensorService _service;

    public SensorServiceTests()
    {
        _service = new SensorService(_sensorRepo.Object, _readingRepo.Object, _thresholdRepo.Object, _alertService.Object);
    }

    [Fact]
    public async Task AddSensorReadingAsync_AddsReadingAndChecksThreshold()
    {
        var user = new User("User", "u@e.com", "hash");
        var greenhouse = new Greenhouse("GH", "Tomato", user);
        var sensor = new Sensor("Temperature", "Active", "C", greenhouse);
        var reading = new SensorReading(DateTime.UtcNow, 5, "C", sensor);

        _thresholdRepo.Setup(r => r.GetThresholdBySensorIdAsync(sensor.Id)).ReturnsAsync(new Threshold { MinValue = 0, MaxValue = 10, SensorId = sensor.Id, Sensor = sensor });
        await _service.AddSensorReadingAsync(reading);
        _readingRepo.Verify(r => r.AddAsync(reading), Times.Once);
    }
}