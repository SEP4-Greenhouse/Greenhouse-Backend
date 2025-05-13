using Domain.Entities;
using Domain.IRepositories;
namespace EFCGreenhouse.Repositories;

public class PlantRepository : BaseRepository<Plant>, IPlantRepository
{
    public PlantRepository(GreenhouseDbContext context) : base(context)
    {
    }
}