using cda_ecf_asp_net.Models;

namespace cda_ecf_asp_net.Repositories;

public interface IParticipantRepository
{
    public Task RegisterToEvent(int id, Participant participant);
}