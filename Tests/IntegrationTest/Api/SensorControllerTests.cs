using System.Net.Http.Json;
using System.Text.Json;
using Domain.DTOs;
using GreenhouseApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Tests.IntegrationTest.Api;

public class SensorControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public SensorControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();

        // Login with existing user
        var login = new LoginRequestDto("bibek@gmail.com", "bibek123");
        var loginResponse = _client.PostAsJsonAsync("/api/auth/login", login).Result;
        loginResponse.EnsureSuccessStatusCode();
        var loginData = loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>().Result;
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginData!.Token);
    }

    [Fact]
    public async Task AddReading_ToExistingSensor_Works()
    {
        int sensorId = 1;
        var readingDto = new SensorReadingDto
        {
            TimeStamp = DateTime.UtcNow,
            Value = 25.5
        };
        var addReadingResponse = await _client.PostAsJsonAsync($"/reading(IotClient)?sensorId={sensorId}", readingDto);
        addReadingResponse.EnsureSuccessStatusCode();

        var latestReadingResponse = await _client.GetAsync($"/api/sensor/{sensorId}/latestBySensor");
        latestReadingResponse.EnsureSuccessStatusCode();
        var latestReading = await latestReadingResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(25.5, latestReading.GetProperty("value").GetDouble());
    }

    [Fact]
    public async Task GetLatestReadingFromAllSensors_Works()
    {
        var response = await _client.GetAsync("/api/sensor/latest/all");
        response.EnsureSuccessStatusCode();
        var readings = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(readings.GetArrayLength() > 0);
    }

    [Fact]
    public async Task GetAllReadingsBySensor_Works()
    {
        var response = await _client.GetAsync("/api/sensor/allReadings");
        response.EnsureSuccessStatusCode();
        var readings = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(readings.EnumerateObject().Any());
    }

    [Fact]
    public async Task GetReadingsByTimestampRange_Works()
    {
        var start = DateTime.UtcNow.AddHours(-1).ToString("o");
        var end = DateTime.UtcNow.AddHours(1).ToString("o");
        var response = await _client.GetAsync($"/api/sensor/range?start={start}&end={end}");
        response.EnsureSuccessStatusCode();
        var readings = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.NotNull(readings);
    }

    [Fact]
    public async Task GetPaginatedReadings_Works()
    {
        int sensorId = 1;
        var response = await _client.GetAsync($"/api/sensor/{sensorId}/paginated?pageNumber=1&pageSize=5");
        response.EnsureSuccessStatusCode();
        var readings = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.NotNull(readings);
    }

    [Fact]
    public async Task GetAverageReading_Works()
    {
        int sensorId = 1;
        var start = DateTime.UtcNow.AddHours(-1).ToString("o");
        var end = DateTime.UtcNow.AddHours(1).ToString("o");
        var response = await _client.GetAsync($"/api/sensor/{sensorId}/average?start={start}&end={end}");
        response.EnsureSuccessStatusCode();
        var avg = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(avg.TryGetProperty("average", out _));
    }
}