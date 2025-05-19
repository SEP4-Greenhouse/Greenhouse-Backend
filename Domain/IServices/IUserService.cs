using Domain.DTOs;
using Domain.Entities;

namespace Domain.IServices;

public interface IUserService 
{
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto> AddUserAsync(UserDto userDto, string hashedPassword);
    //Task UpdateUserAsync(UserDto userDto);
    Task DeleteUserAsync(int id);
    Task UpdatePasswordAsync(int userId, string newPassword);
    Task UpdateNameAsync(int userId, string newName);
}