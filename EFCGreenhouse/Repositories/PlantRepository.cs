using Domain.Entities;
using Domain.IRepositories;

namespace EFCGreenhouse.Repositories;

public class PlantRepository(GreenhouseDbContext context) : BaseRepository<Plant>(context), IPlantRepository;