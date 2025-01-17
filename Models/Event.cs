using System.ComponentModel.DataAnnotations;

namespace cda_ecf_asp_net.Models;

public class Event
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    public string Location { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
    
    public ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();
}