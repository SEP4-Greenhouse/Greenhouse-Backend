using Domain.DTOs;

namespace Domain.IServices;

public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(int id);
    Task DeleteUserAsync(int id);
    Task UpdatePasswordAsync(int userId, string newPassword);
    Task UpdateNameAsync(int userId, string newName);
}