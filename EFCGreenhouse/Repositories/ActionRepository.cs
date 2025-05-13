using Domain.Entities;
using Domain.IRepositories;

namespace EFCGreenhouse.Repositories;

public class ActionRepository(GreenhouseDbContext context)
    : BaseRepository<ControllerAction>(context), IActionRepository;