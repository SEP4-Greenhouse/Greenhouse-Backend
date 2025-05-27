using System.Net.Http.Json;
using System.Text.Json;
using Domain.DTOs;
using GreenhouseApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Tests.IntegrationTest.Api;

public class GreenhouseControllerTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Greenhouse_FullLifecycle_Works()
    {
        // Register and login user
        var uniqueEmail = $"ghuser_{Guid.NewGuid()}@example.com";
        var user = new CreateUserDto("GHUser", uniqueEmail, "TestPassword123");
        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", user);
        registerResponse.EnsureSuccessStatusCode();

        var login = new LoginRequestDto(uniqueEmail, "TestPassword123");
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", login);
        loginResponse.EnsureSuccessStatusCode();
        var loginData = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginData!.Token);

        int userId = 1;

        // Create greenhouse
        var greenhouseDto = new GreenhouseDto("TestGreenhouse", "Tomato", userId);
        var createResponse = await _client.PostAsJsonAsync("/api/greenhouse", greenhouseDto);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        int greenhouseId = created.GetProperty("id").GetInt32();

        // Add plant
        var plantDto = new PlantDto("Cucumber", DateTime.UtcNow.Date, "Seedling");
        var addPlantResponse = await _client.PostAsJsonAsync($"/api/greenhouse/{greenhouseId}/plants", plantDto);
        addPlantResponse.EnsureSuccessStatusCode();
        var plant = await addPlantResponse.Content.ReadFromJsonAsync<JsonElement>();
        int plantId = plant.GetProperty("id").GetInt32();

        // Update plant growth stage
        var updatePlantStageResponse = await _client.PutAsJsonAsync($"/api/greenhouse/{greenhouseId}/plants/{plantId}/growthStage", "\"Vegetative\"");
        updatePlantStageResponse.EnsureSuccessStatusCode();

        // Update plant species
        var updatePlantSpeciesResponse = await _client.PutAsJsonAsync($"/api/greenhouse/{greenhouseId}/plants/{plantId}/species", "\"Pepper\"");
        updatePlantSpeciesResponse.EnsureSuccessStatusCode();

        // Get plants
        var getPlantsResponse = await _client.GetAsync($"/api/greenhouse/{greenhouseId}/plants");
        getPlantsResponse.EnsureSuccessStatusCode();

        // Add sensor
        var sensorDto = new SensorDto("Temperature", "Active", "Celsius");
        var addSensorResponse = await _client.PostAsJsonAsync($"/api/greenhouse/{greenhouseId}/sensors", sensorDto);
        addSensorResponse.EnsureSuccessStatusCode();

        // Get sensors
        var getSensorsResponse = await _client.GetAsync($"/api/greenhouse/{greenhouseId}/sensors");
        getSensorsResponse.EnsureSuccessStatusCode();
        var sensors = await getSensorsResponse.Content.ReadFromJsonAsync<JsonElement>();
        int sensorId = sensors[0].GetProperty("id").GetInt32();

        // Update sensor status
        var updateSensorStatusResponse = await _client.PutAsJsonAsync($"/api/greenhouse/{greenhouseId}/sensors/{sensorId}/status", "\"Inactive\"");
        updateSensorStatusResponse.EnsureSuccessStatusCode();

        // Add actuator
        var actuatorDto = new ActuatorDto("waterpump", "Active");        var addActuatorResponse = await _client.PostAsJsonAsync($"/api/greenhouse/{greenhouseId}/actuators", actuatorDto);
        addActuatorResponse.EnsureSuccessStatusCode();

        // Get actuators
        var getActuatorsResponse = await _client.GetAsync($"/api/greenhouse/{greenhouseId}/actuators");
        getActuatorsResponse.EnsureSuccessStatusCode();
        var actuators = await getActuatorsResponse.Content.ReadFromJsonAsync<JsonElement>();
        int actuatorId = actuators[0].GetProperty("id").GetInt32();

        // Update actuator status
        var updateActuatorStatusResponse = await _client.PutAsJsonAsync($"/api/greenhouse/{greenhouseId}/actuators/{actuatorId}/status", "\"Inactive\"");
        updateActuatorStatusResponse.EnsureSuccessStatusCode();

        // Delete actuator
        var deleteActuatorResponse = await _client.DeleteAsync($"/api/greenhouse/{greenhouseId}/actuators/{actuatorId}");
        deleteActuatorResponse.EnsureSuccessStatusCode();

        // Delete sensor
        var deleteSensorResponse = await _client.DeleteAsync($"/api/greenhouse/{greenhouseId}/sensors/{sensorId}");
        deleteSensorResponse.EnsureSuccessStatusCode();

        // Delete plant
        var deletePlantResponse = await _client.DeleteAsync($"/api/greenhouse/{greenhouseId}/plants/{plantId}");
        deletePlantResponse.EnsureSuccessStatusCode();

        // Delete greenhouse
        var deleteGreenhouseResponse = await _client.DeleteAsync($"/api/greenhouse/{greenhouseId}");
        deleteGreenhouseResponse.EnsureSuccessStatusCode();
    }
}