namespace Domain.DTOs;

public class CreateUserDto
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }

    public CreateUserDto(string name, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.");
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.");
        if (!email.Contains("@"))
            throw new ArgumentException("Invalid email format.");
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty.");

        Name = name;
        Email = email;
        Password = password;
    }
}