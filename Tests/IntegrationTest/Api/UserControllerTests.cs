using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Domain.DTOs;
using GreenhouseApi;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Tests.IntegrationTest.Api;

public class UserControllerTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    private async Task AuthenticateAsync(string email, string password, string name)
    {
        var registerDto = new CreateUserDto(name, email, password);
        await _client.PostAsJsonAsync("/api/auth/register", registerDto);

        var login = new LoginRequestDto(email, password);
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", login);
        loginResponse.EnsureSuccessStatusCode();
        var loginData = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginData!.Token);
    }

    [Fact]
    public async Task GetMyUserInfo_Works()
    {
        await AuthenticateAsync("getinfo@example.com", "test123", "GetInfoUser");
        var response = await _client.GetAsync("/api/user/me");
        response.EnsureSuccessStatusCode();
        var user = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("getinfo@example.com", user.GetProperty("email").GetString());
    }

    [Fact]
    public async Task UpdateUserName_Works()
    {
        await AuthenticateAsync("updatename@example.com", "test123", "UpdateNameUser");
        var newName = "UpdatedName";
        var response = await _client.PutAsJsonAsync("/api/user/name", newName);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var userResponse = await _client.GetAsync("/api/user/me");
        userResponse.EnsureSuccessStatusCode();
        var user = await userResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(newName, user.GetProperty("name").GetString());
    }

    [Fact]
    public async Task UpdatePassword_Works()
    {
        // Use a unique email to avoid conflicts
        var uniqueEmail = $"updatepass_{Guid.NewGuid()}@example.com";
        var registerDto = new CreateUserDto("UpdatePassUser", uniqueEmail, "test123");
        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerDto);
        if (!registerResponse.IsSuccessStatusCode)
        {
            var error = await registerResponse.Content.ReadAsStringAsync();
            throw new Exception($"Registration failed: {error}");
        }

        var login = new LoginRequestDto(uniqueEmail, "test123");
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", login);
        if (!loginResponse.IsSuccessStatusCode)
        {
            var error = await loginResponse.Content.ReadAsStringAsync();
            throw new Exception($"Login failed: {error}");
        }
        var loginData = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginData!.Token);

        var newPassword = "newpassword123";
        var response = await _client.PutAsJsonAsync("/api/user/password", newPassword);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteUser_Works()
    {
        await AuthenticateAsync("deleteme@example.com", "delete123", "DeleteMe");
        var deleteResponse = await _client.DeleteAsync("/api/user");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }
}