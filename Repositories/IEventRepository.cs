using cda_ecf_asp_net.Models;

namespace cda_ecf_asp_net.Repositories;

public interface IEventRepository
{
    public Task<List<Event>> GetAll();
    public Task<Event?> GetById(int id);
    public Task<Event?> Create(Event? @event);
    public Task<Event> Update(Event @event);
    public Task Delete(Event @event);
    public Task<long> GetTotalEventsAsync();
    public Task<long> GetUpcomingEventsAsync();
    public Task<long> GetPastEventsAsync();
    public Task<double> GetAverageEventDurationAsync();
    public Task<double> GetAverageParticipantsPerEventAsync();
    public Task<string?> GetMostPopularEventAsync();
}