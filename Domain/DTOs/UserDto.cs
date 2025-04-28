namespace Domain.DTOs;

public class UserDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }

    public UserDto(int id, string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.");
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.");
        if (!email.Contains("@"))
            throw new ArgumentException("Invalid email format.");

        Id = id;
        Name = name;
        Email = email;
    }
}