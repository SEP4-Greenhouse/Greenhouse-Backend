namespace Domain.Entities;

public class User
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string HashedPassword { get; private set; }

    public User(string name, string email, string hashedPassword)
    {
        Name = name;
        Email = email;
        HashedPassword = hashedPassword;
    }

    private User() { } // Required by EF Core
    
    public void ChangePassword(string newHashedPassword)
    {
        HashedPassword = newHashedPassword;
    }

    public void UpdateEmail(string newEmail)
    {
        if (string.IsNullOrWhiteSpace(newEmail))
            throw new ArgumentException("Email cannot be empty.");

        if (newEmail == Email)
            throw new InvalidOperationException("New email cannot be the same as the current email.");

        if (!newEmail.Contains("@"))
            throw new ArgumentException("Invalid email format.");

        Email = newEmail;
    }

    public void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name cannot be empty.");
        Name = newName;
    }
}