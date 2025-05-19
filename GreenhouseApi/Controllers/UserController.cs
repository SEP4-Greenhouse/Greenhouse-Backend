using Domain.DTOs;
using Domain.IServices;
using Microsoft.AspNetCore.Mvc;

namespace GreenhouseApi.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var userDto = await userService.GetUserByIdAsync(id);
        if (userDto == null)
            return NotFound("User not found.");

        var userResponse = new {
            id = userDto.Id,
            name = userDto.Name,
            email = userDto.Email,
        };

        return Ok(userResponse);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await userService.GetAllUsersAsync();
        
        var userResponses = users.Select(user => new {
            id = user.Id,
            name = user.Name,
            email = user.Email,
        }).ToList();
    
        return Ok(userResponses);
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] CreateUserDto createUserDto)
    {
        try
        {
            var userDto = new UserDto(0, createUserDto.Name, createUserDto.Email);
            var createdUser = await userService.AddUserAsync(userDto, createUserDto.Password);

            var userResponse = new {
                id = createdUser.Id,
                name = createdUser.Name,
                email = createdUser.Email
            };

            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, userResponse);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            await userService.DeleteUserAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpPut("{id}/name")]
    public async Task<IActionResult> UpdateUserName(int id, [FromBody] string newName)
    {
        try
        {
            await userService.UpdateNameAsync(id, newName);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}/password")]
    public async Task<IActionResult> UpdateUserPassword(int id, [FromBody] string newPassword)
    {
        try
        {
            await userService.UpdatePasswordAsync(id, newPassword);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}