using Domain.Entities;
using Domain.IRepositories;
using EFCGreenhouse;
using EFCGreenhouse.Repositories;
using Microsoft.Extensions.Logging;

public class ActionRepository : BaseRepository<ControllerAction>, IActionRepository
{
    private readonly ILogger<ActionRepository> _logger;

    public ActionRepository(GreenhouseDbContext context, ILogger<ActionRepository> logger) 
        : base(context)
    {
        _logger = logger;
    }
}