using Domain.Entities;

namespace Domain.IRepositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<bool> ExistsByEmailAsync(string email);
    Task<User?> GetByEmailAsync(string loginRequestEmail);
}