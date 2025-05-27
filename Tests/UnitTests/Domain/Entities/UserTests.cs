using Domain.Entities;

namespace Tests.UnitTests.Domain.Entities;

public class UserTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var user = new User("Name", "email@example.com", "hash");
        Assert.Equal("Name", user.Name);
        Assert.Equal("email@example.com", user.Email);
        Assert.Equal("hash", user.HashedPassword);
    }

    [Fact]
    public void ChangePassword_UpdatesPassword()
    {
        var user = new User("Name", "email@example.com", "hash");
        user.ChangePassword("newhash");
        Assert.Equal("newhash", user.HashedPassword);
    }

    [Fact]
    public void ChangeName_ThrowsArgumentException_WhenNullOrWhitespace()
    {
        var user = new User("Name", "email@example.com", "hash");
        Assert.Throws<ArgumentException>(() => user.ChangeName(null));
        Assert.Throws<ArgumentException>(() => user.ChangeName(""));
        Assert.Throws<ArgumentException>(() => user.ChangeName("   "));
    }

    [Fact]
    public void ChangeName_UpdatesName_WhenValid()
    {
        var user = new User("Name", "email@example.com", "hash");
        user.ChangeName("NewName");
        Assert.Equal("NewName", user.Name);
    }
}