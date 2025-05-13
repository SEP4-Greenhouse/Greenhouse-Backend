using Domain.Entities;
using Domain.IRepositories;

namespace EFCGreenhouse.Repositories;

public class GreenhouseRepository(GreenhouseDbContext context)
    : BaseRepository<Greenhouse>(context), IGreenhouseRepository;