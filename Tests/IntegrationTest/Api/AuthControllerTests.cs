using System.Net.Http.Json;
using Domain.DTOs;
using GreenhouseApi;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Tests.IntegrationTest.Api;

public class AuthControllerTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Register_ReturnsCreated()
    {
        var uniqueEmail = $"testuser_{Guid.NewGuid()}@example.com";
        var user = new CreateUserDto("TestUser", uniqueEmail, "TestPassword123");
        var response = await _client.PostAsJsonAsync("/api/auth/register", user);
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Login_ReturnsOk_WhenCredentialsAreValid()
    {
        var user = new CreateUserDto("TestUser", "testuser@example.com", "TestPassword123");
        await _client.PostAsJsonAsync("/api/auth/register", user);

        var login = new LoginRequestDto("testuser@example.com", "TestPassword123");
        var response = await _client.PostAsJsonAsync("/api/auth/login", login);

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        Assert.False(string.IsNullOrEmpty(loginResponse?.Token));
    }

    [Fact]
    public async Task HealthCheck_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/auth/health");
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }
}