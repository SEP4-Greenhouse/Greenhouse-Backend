using Domain.Entities;
using Domain.IRepositories;

namespace EFCGreenhouse.Repositories;

public class ActuatorActionRepository(GreenhouseDbContext context)
    : BaseRepository<ActuatorAction>(context), IActuatorActionRepository;