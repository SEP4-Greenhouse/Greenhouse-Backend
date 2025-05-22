namespace Domain.IServices;

using System.Threading.Tasks;
using DTOs;
using Entities;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest);
    Task<UserDto> RegisterAsync(CreateUserDto createUserDto);
    Task<string> GenerateTokenAsync(User user);
    bool VerifyPassword(string plainPassword, string hashedPassword);
}