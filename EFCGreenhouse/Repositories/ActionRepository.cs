using Domain.Entities;
using Domain.IRepositories;
using EFCGreenhouse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class ActionRepository : IActionRepository
{
    private readonly GreenhouseDbContext _context;
    private readonly ILogger<ActionRepository> _logger;

    public ActionRepository(GreenhouseDbContext context, ILogger<ActionRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ControllerAction?> GetByIdAsync(int id)
    {
        return await _context.ControllerActions.FindAsync(id);
    }

    public async Task<IEnumerable<ControllerAction>> GetAllAsync()
    {
        return await _context.ControllerActions
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(ControllerAction controllerAction)
    {
        try
        {
            await _context.ControllerActions.AddAsync(controllerAction);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding controller action {Id}", controllerAction.Id);
            throw;
        }
    }

    public async Task UpdateAsync(ControllerAction controllerAction)
    {
        try
        {
            _context.Entry(controllerAction).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating controller action {Id}", controllerAction.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        var action = await GetByIdAsync(id);
        if (action != null)
        {
            try
            {
                _context.ControllerActions.Remove(action);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting controller action {Id}", id);
                throw;
            }
        }
    }
}