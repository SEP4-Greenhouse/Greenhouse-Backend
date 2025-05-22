using System.Security.Claims;
using Domain.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenhouseApi.Controllers;

[Authorize]
[ApiController]
[Route("api/user")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetMyUserInfo()
    {
        var userId = GetUserIdFromClaims();
        if (userId == null) return Unauthorized();

        var userDto = await userService.GetUserByIdAsync(userId.Value);
        if (userDto == null) return NotFound("User not found.");

        return Ok(new
        {
            id = userDto.Id,
            name = userDto.Name,
            email = userDto.Email
        });
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUser()
    {
        var userId = GetUserIdFromClaims();
        if (userId == null) return Unauthorized();

        await userService.DeleteUserAsync(userId.Value);
        return NoContent();
    }

    [HttpPut("name")]
    public async Task<IActionResult> UpdateUserName([FromBody] string newName)
    {
        var userId = GetUserIdFromClaims();
        if (userId == null) return Unauthorized();

        await userService.UpdateNameAsync(userId.Value, newName);
        return NoContent();
    }

    [HttpPut("password")]
    public async Task<IActionResult> UpdatePassword([FromBody] string newPassword)
    {
        var userId = GetUserIdFromClaims();
        if (userId == null) return Unauthorized();

        await userService.UpdatePasswordAsync(userId.Value, newPassword);
        return NoContent();
    }

    private int? GetUserIdFromClaims()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return null;
        return int.TryParse(userIdClaim.Value, out var userId) ? userId : null;
    }
}