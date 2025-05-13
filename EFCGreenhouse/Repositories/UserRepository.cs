using Domain.Entities;
using Domain.IRepositories;
using EFCGreenhouse.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EFCGreenhouse.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(GreenhouseDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await DbSet.AnyAsync(u => u.Email == email);
    }
}   