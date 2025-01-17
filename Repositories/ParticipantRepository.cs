using cda_ecf_asp_net.Models;

namespace cda_ecf_asp_net.Repositories;

public class ParticipantRepository : IParticipantRepository
{
    private readonly ApplicationDbContext _context;

    public ParticipantRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task RegisterToEvent(int eventId, Participant participant)
    {
        // Add participant to database
        _context.Participants.Add(participant);
        await _context.SaveChangesAsync();

        // Link participant to event
        var eventParticipant = new EventParticipant
        {
            EventId = eventId,
            ParticipantId = participant.ParticipantId,
            RegistrationDate = DateTime.Now
        };

        _context.EventParticipants.Add(eventParticipant);
        await _context.SaveChangesAsync();
    }

}