using System.Net;
using System.Net.Http.Json;
using Domain.DTOs;
using Domain.Entities;
using GreenhouseApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Tests.IntegrationTest.Api;

public class AlertsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AlertsControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();

        var login = new LoginRequestDto("bibek@gmail.com", "bibek123");
        var loginResponse = _client.PostAsJsonAsync("/api/auth/login", login).Result;
        loginResponse.EnsureSuccessStatusCode();
        var loginData = loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>().Result;
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginData!.Token);
    }

    [Fact]
    public async Task GetAllAlerts_ReturnsOkAndAlerts()
    {
        var response = await _client.GetAsync("/api/alerts/all");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var alerts = await response.Content.ReadFromJsonAsync<List<AlertDto>>();
        Assert.NotNull(alerts);
    }

    [Fact]
    public async Task GetAlertsByType_ReturnsOkAndAlerts()
    {
        var type = Alert.AlertType.Actuator;
        var response = await _client.GetAsync($"/api/alerts/type/{type}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var alerts = await response.Content.ReadFromJsonAsync<List<AlertDto>>();
        Assert.NotNull(alerts);
    }

    [Fact]
    public async Task GetAlertsByDateRange_ReturnsOkAndAlerts()
    {
        var start = DateTime.UtcNow.AddDays(-1).ToString("o");
        var end = DateTime.UtcNow.AddDays(1).ToString("o");
        var response = await _client.GetAsync($"/api/alerts/range?start={start}&end={end}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var alerts = await response.Content.ReadFromJsonAsync<List<AlertDto>>();
        Assert.NotNull(alerts);
    }
}