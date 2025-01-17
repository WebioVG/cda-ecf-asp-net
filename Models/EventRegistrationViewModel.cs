using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace cda_ecf_asp_net.Models;

public class EventRegistrationViewModel
{
    public Event? Event { get; set; }
    public Participant Participant { get; set; } = new Participant();
}