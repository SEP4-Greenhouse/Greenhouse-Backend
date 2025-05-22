using Domain.DTOs;

namespace Domain.IServices;

public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto> AddUserAsync(UserDto userDto, string hashedPassword);
    Task DeleteUserAsync(int id);
    Task UpdatePasswordAsync(int userId, string newPassword);
    Task UpdateNameAsync(int userId, string newName);
}