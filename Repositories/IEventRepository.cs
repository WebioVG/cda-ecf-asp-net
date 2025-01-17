using cda_ecf_asp_net.Models;

namespace cda_ecf_asp_net.Repositories;

public interface IEventRepository
{
    public Task<List<Event>> GetAll();
    public Task<Event?> GetById(int id);
    public Task<Event?> Create(Event? @event);
    public Task<Event> Update(Event @event);
    public Task Delete(Event @event);
}