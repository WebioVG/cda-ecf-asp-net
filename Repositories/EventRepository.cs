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
    
    public async Task<long> GetTotalEventsAsync()
    {
        return await _context.Events.LongCountAsync();
    }

    public async Task<long> GetUpcomingEventsAsync()
    {
        return await _context.Events.CountAsync(e => e.StartDate > DateTime.UtcNow);
    }

    public async Task<long> GetPastEventsAsync()
    {
        return await _context.Events.CountAsync(e => e.EndDate < DateTime.UtcNow);
    }

    public async Task<double> GetAverageEventDurationAsync()
    {
        var durations = await _context.Events
            .Select(e => EF.Functions.DateDiffHour(e.StartDate, e.EndDate))
            .ToListAsync();

        return durations.Any() ? durations.Average() : 0;
    }

    public async Task<double> GetAverageParticipantsPerEventAsync()
    {
        var participantCounts = await _context.Events
            .GroupJoin(
                _context.EventParticipants,
                e => e.Id,
                ep => ep.EventId,
                (e, participants) => participants.Count()
            )
            .ToListAsync();

        return participantCounts.Any() ? participantCounts.Average() : 0;
    }

    public async Task<string?> GetMostPopularEventAsync()
    {
        var mostPopular = await _context.Events
            .GroupJoin(
                _context.EventParticipants,
                e => e.Id,
                ep => ep.EventId,
                (e, participants) => new { e.Title, ParticipantCount = participants.Count() }
            )
            .OrderByDescending(e => e.ParticipantCount)
            .FirstOrDefaultAsync();

        return mostPopular?.Title;
    }
}