using System.Security.Cryptography;
using System.Text;
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

    public async Task AddUserAsync(UserDto userDto, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty.");

        if (await userRepository.ExistsByEmailAsync(userDto.Email))
            throw new InvalidOperationException("A user with the same email already exists.");

        var hashedPassword = HashPassword(password);

        var user = new User(userDto.Name, userDto.Email, hashedPassword);
        await userRepository.AddAsync(user);
    }

    public async Task UpdateUserAsync(UserDto userDto)
    {
        if (userDto.Id <= 0)
            throw new ArgumentException("User ID must be greater than zero.");

        var user = await userRepository.GetByIdAsync(userDto.Id);
        if (user == null)
            throw new KeyNotFoundException("User not found.");

        if (user.Email != userDto.Email && await userRepository.ExistsByEmailAsync(userDto.Email))
            throw new InvalidOperationException("A user with the same email already exists.");

        user.ChangeName(userDto.Name);
        user.UpdateEmail(userDto.Email);
        await userRepository.UpdateAsync(user);
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

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}