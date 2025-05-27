using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Domain.DTOs;
using GreenhouseApi;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Tests.IntegrationTest.Api;

public class MlModelControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public MlModelControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();

        var login = new LoginRequestDto("bibek@gmail.com", "bibek123");
        var loginResponse = _client.PostAsJsonAsync("/api/auth/login", login).Result;
        loginResponse.EnsureSuccessStatusCode();
        var loginData = loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>().Result;
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginData!.Token);
    }

    [Fact]
    public async Task PredictNextWateringTime_Works()
    {
        var greenhouseDto = new GreenhouseDto("MLGH", "Tomato", 1);
        var createGreenhouseResponse = await _client.PostAsJsonAsync("/api/greenhouse", greenhouseDto);
        createGreenhouseResponse.EnsureSuccessStatusCode();
        var createdGreenhouse = await createGreenhouseResponse.Content.ReadFromJsonAsync<JsonElement>();
        int greenhouseId = createdGreenhouse.GetProperty("id").GetInt32();

        var plantDto = new PlantDto("Cucumber", DateTime.UtcNow.Date, "Seedling");
        var addPlantResponse = await _client.PostAsJsonAsync($"/api/greenhouse/{greenhouseId}/plants", plantDto);
        addPlantResponse.EnsureSuccessStatusCode();
        var plant = await addPlantResponse.Content.ReadFromJsonAsync<JsonElement>();
        int plantId = plant.GetProperty("id").GetInt32();

        var predictResponse = await _client.PostAsync($"/api/ml/predict-next-watering-time/{plantId}", null);
        Assert.Equal(HttpStatusCode.OK, predictResponse.StatusCode);
        var prediction = await predictResponse.Content.ReadFromJsonAsync<PredictionResultDto>();
        Assert.NotNull(prediction);
    }

    [Fact]
    public async Task GetAllPredictionLogs_Works()
    {
        var response = await _client.GetAsync("/api/ml/prediction-logs");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var logs = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.NotNull(logs);
    }
}