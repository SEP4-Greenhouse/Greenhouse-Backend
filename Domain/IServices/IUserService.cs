using Domain.DTOs;
using Domain.Entities;

namespace Domain.IServices;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int id);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task AddUserAsync(UserDto userDto, string hashedPassword);
    Task UpdateUserAsync(UserDto userDto);
    Task DeleteUserAsync(int id);
}