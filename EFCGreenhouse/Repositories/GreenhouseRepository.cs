using Domain.Entities;
using Domain.IRepositories;
using EFCGreenhouse.Repositories;

namespace EFCGreenhouse.Repositories;

public class GreenhouseRepository : BaseRepository<Greenhouse>, IGreenhouseRepository
{
    public GreenhouseRepository(GreenhouseDbContext context) : base(context)
    {
    }
}