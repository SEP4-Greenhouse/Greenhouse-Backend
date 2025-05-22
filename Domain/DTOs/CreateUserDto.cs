namespace Domain.DTOs;

public class CreateUserDto(string name, string email, string password)
{
    public string Name { get; init; } = name;
    public string Email { get; init; } = email;
    public string Password { get; init; } = password;
}