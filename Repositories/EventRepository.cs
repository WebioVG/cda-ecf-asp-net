using cda_ecf_asp_net.Models;
using Microsoft.EntityFrameworkCore;

namespace cda_ecf_asp_net.Repositories;

public class EventRepository : IEventRepository
{
    private readonly ApplicationDbContext _context;

    public EventRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Event>> GetAll()
    {
        return await _context.Events
            .OrderBy(e => e.StartDate)
            .ToListAsync();
    }

    public async Task<Event?> GetById(int id)
    {
        return await _context.Events.FindAsync(id);
    }

    public async Task<Event?> Create(Event? @event)
    {
        // Add a new event to the database
        _context.Events.Add(@event);
        await _context.SaveChangesAsync();
        return @event;
    }

    public async Task<Event> Update(Event @event)
    {
        var existingEvent = await _context.Events.FindAsync(@event.Id);
        if (existingEvent == null)
        {
            throw new KeyNotFoundException($"Event with ID {@event.Id} not found.");
        }

        existingEvent.Title = @event.Title;
        existingEvent.Description = @event.Description;
        existingEvent.StartDate = @event.StartDate;
        existingEvent.EndDate = @event.EndDate;
        existingEvent.Location = @event.Location;

        _context.Events.Update(existingEvent);
        await _context.SaveChangesAsync();
        return existingEvent;
    }

    public async Task Delete(Event @event)
    {
        var existingEvent = await _context.Events.FindAsync(@event.Id);
        if (existingEvent == null)
        {
            throw new KeyNotFoundException($"Event with ID {@event.Id} not found.");
        }

        _context.Events.Remove(existingEvent);
        await _context.SaveChangesAsync();
    }
}