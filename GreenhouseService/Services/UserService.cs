using Domain.DTOs;
using Domain.Entities;
using Domain.IRepositories;
using Domain.IServices;

namespace GreenhouseService.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("User ID must be greater than zero.");

        var user = await userRepository.GetByIdAsync(id);
        return user == null ? null : new UserDto(user.Id, user.Name, user.Email);
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await userRepository.GetAllAsync();
        return users.Select(user => new UserDto(user.Id, user.Name, user.Email));
    }

    public async Task<UserDto> AddUserAsync(UserDto userDto, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty.");

        if (await userRepository.ExistsByEmailAsync(userDto.Email))
            throw new InvalidOperationException("A user with the same email already exists.");

        var hashedPassword = HashPassword(password);

        var user = new User(userDto.Name, userDto.Email, hashedPassword);
        var createdUser = await userRepository.AddAsync(user);

        return new UserDto(createdUser.Id, createdUser.Name, createdUser.Email);
    }

    public async Task DeleteUserAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("User ID must be greater than zero.");

        var user = await userRepository.GetByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException("User not found.");

        await userRepository.DeleteAsync(id);
    }

    public async Task UpdatePasswordAsync(int userId, string newPassword)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than zero.");
        if (string.IsNullOrWhiteSpace(newPassword))
            throw new ArgumentException("Password cannot be empty.");

        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("User not found.");

        var hashedPassword = HashPassword(newPassword);
        user.ChangePassword(hashedPassword);
        await userRepository.UpdateAsync(user);
    }

    public async Task UpdateNameAsync(int userId, string newName)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than zero.");
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name cannot be empty.");

        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("User not found.");

        user.ChangeName(newName);
        await userRepository.UpdateAsync(user);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}