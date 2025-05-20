using Domain.DTOs;
using Domain.IServices;
using Microsoft.AspNetCore.Mvc;

namespace GreenhouseApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto loginRequest)
    {
        var response = await _authService.LoginAsync(loginRequest);
        if (response == null)
            return Unauthorized();

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(CreateUserDto createUserDto)
    {
        if (string.IsNullOrWhiteSpace(createUserDto.Email) ||
            string.IsNullOrWhiteSpace(createUserDto.Password) ||
            string.IsNullOrWhiteSpace(createUserDto.Name))
        {
            return BadRequest("Name, email, and password are required.");
        }
        try
        {
            var user = await _authService.RegisterAsync(createUserDto);
            return CreatedAtAction(nameof(Register), user);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok("Backend is alive");
    }

}