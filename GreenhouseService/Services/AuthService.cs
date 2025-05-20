using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.DTOs;
using Domain.Entities;
using Domain.IRepositories;
using Domain.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GreenhouseService.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepo;

    public AuthService(IConfiguration config, IUserRepository userRepo)
    {
        _config = config;
        _userRepo = userRepo;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest)
    {
        var user = await _userRepo.GetByEmailAsync(loginRequest.Email);

        if (user == null || !VerifyPassword(loginRequest.Password, user.HashedPassword))
            return null;


        var token = await GenerateTokenAsync(user);
        return new LoginResponseDto
        {
            Token = token,
            Expiry = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiresInMinutes"]!))
        };
    }


    public Task<string> GenerateTokenAsync(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiresInMinutes"]!)),
            signingCredentials: creds
        );

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<UserDto> RegisterAsync(CreateUserDto createUserDto)
    {
        if (await _userRepo.ExistsByEmailAsync(createUserDto.Email))
            throw new InvalidOperationException("User with this email already exists.");

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
        var user = new User(createUserDto.Name, createUserDto.Email, hashedPassword);

        var createdUser = await _userRepo.AddAsync(user);
        return new UserDto(createdUser.Id, createdUser.Name, createdUser.Email);
    }

    public bool VerifyPassword(string plainPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }
}