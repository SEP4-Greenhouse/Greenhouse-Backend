namespace Domain.DTOs;

public class LoginResponseDto
{
    public string Token { get; set; } = null!;
    public DateTime Expiry { get; set; }
}