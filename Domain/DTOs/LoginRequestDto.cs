namespace Domain.DTOs;

public class LoginRequestDto(string email, string password)
{
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
}