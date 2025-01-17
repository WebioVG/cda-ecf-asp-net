using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cda_ecf_asp_net.Models;

public class EventParticipant
{
    [Key, ForeignKey(nameof(Event))]
    public int EventId { get; set; }
    public Event Event { get; set; }

    [Key, ForeignKey(nameof(Participant))]
    public int ParticipantId { get; set; }
    public Participant Participant { get; set; }

    [Required]
    public DateTime RegistrationDate { get; set; } = DateTime.Now;
    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Registered";
}