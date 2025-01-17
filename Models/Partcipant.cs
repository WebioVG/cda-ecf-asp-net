using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cda_ecf_asp_net.Models;

public class Participant
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ParticipantId { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(255)]
    [EmailAddress]
    public string Email { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [Required]
    public Guid RegistrationToken { get; set; } = Guid.NewGuid();

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();
}