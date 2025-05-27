using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Domain.DTOs;
using GreenhouseApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Tests.IntegrationTest.Api;

public class ActuatorControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ActuatorControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();

        var login = new LoginRequestDto("bibek@gmail.com", "bibek123");
        var loginResponse = _client.PostAsJsonAsync("/api/auth/login", login).Result;
        loginResponse.EnsureSuccessStatusCode();
        var loginData = loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>().Result;
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginData!.Token);
    }

    [Fact]
    public async Task TriggerActuatorAction_Works()
    {
        var greenhouseDto = new GreenhouseDto("GH", "Tomato", 1);
        var createGreenhouseResponse = await _client.PostAsJsonAsync("/api/greenhouse", greenhouseDto);
        createGreenhouseResponse.EnsureSuccessStatusCode();
        var createdGreenhouse = await createGreenhouseResponse.Content.ReadFromJsonAsync<JsonElement>();
        int greenhouseId = createdGreenhouse.GetProperty("id").GetInt32();

        var actuatorDto = new ActuatorDto("waterpump", "Active");
        var addActuatorResponse = await _client.PostAsJsonAsync($"/api/greenhouse/{greenhouseId}/actuators", actuatorDto);
        addActuatorResponse.EnsureSuccessStatusCode();

        var actuatorsResponse = await _client.GetAsync($"/api/greenhouse/{greenhouseId}/actuators");
        actuatorsResponse.EnsureSuccessStatusCode();
        var actuators = await actuatorsResponse.Content.ReadFromJsonAsync<JsonElement>();
        int actuatorId = actuators[0].GetProperty("id").GetInt32();

        var actionDto = new ActuatorActionDto(2.5, "SetFlowRate") { Timestamp = DateTime.UtcNow };
        var triggerResponse = await _client.PostAsJsonAsync($"/api/actuator/{actuatorId}/action", actionDto);
        Assert.Equal(HttpStatusCode.OK, triggerResponse.StatusCode);
    }
}