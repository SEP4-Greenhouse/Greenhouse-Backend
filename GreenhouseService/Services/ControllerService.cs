using Domain.Entities;
using Domain.IServices;
using Microsoft.EntityFrameworkCore;

namespace GreenhouseService.Services;

public class ControllerService : IControllerService
{
    private readonly DbContext _context;

    public ControllerService(DbContext context)
    {
        _context = context;
    }

    public async Task<Controller> GetControllerByIdAsync(int id)
    {
        var controller = await _context.Set<Controller>().FindAsync(id);
        if (controller == null)
            throw new KeyNotFoundException("Controller not found.");
        return controller;
    }

    public async Task<IEnumerable<Controller>> GetControllersByGreenhouseIdAsync(int greenhouseId)
    {
        return await _context.Set<Controller>()
            .Where(c => c.GreenhouseId == greenhouseId)
            .ToListAsync();
    }

    public async Task<Controller> CreateControllerAsync(Controller controller)
    {
        if (controller == null)
            throw new ArgumentNullException(nameof(controller));

        if (await _context.Set<Controller>().AnyAsync(c => c.Id == controller.Id))
            throw new ArgumentException("A controller with the same ID already exists.");

        _context.Set<Controller>().Add(controller);
        await _context.SaveChangesAsync();
        return controller;
    }

    public async Task<ControllerAction> TriggerControllerActionAsync(int controllerId, string actionType, double value)
    {
        var controller = await GetControllerByIdAsync(controllerId);
        return controller.InitiateAction(DateTime.UtcNow, actionType, value);
    }

    public async Task UpdateControllerStatusAsync(int controllerId, string newStatus)
    {
        var controller = await GetControllerByIdAsync(controllerId);
        controller.UpdateStatus(newStatus);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ControllerAction>> GetControllerActionsAsync(int controllerId)
    {
        var controller = await GetControllerByIdAsync(controllerId);
        return controller.Actions;
    }

    public async Task DeleteControllerAsync(int controllerId)
    {
        var controller = await GetControllerByIdAsync(controllerId);
        _context.Set<Controller>().Remove(controller);
        await _context.SaveChangesAsync();
    }
}