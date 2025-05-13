using Domain.Entities;
using Domain.IRepositories;

namespace EFCGreenhouse.Repositories;

public class SensorRepository(GreenhouseDbContext context) : BaseRepository<Sensor>(context), ISensorRepository;