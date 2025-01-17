namespace cda_ecf_asp_net.Models;

public class StatsViewModel
{
    public long TotalEvents { get; set; }
    public long UpcomingEvents { get; set; }
    public long PastEvents { get; set; }
    public double AverageDurationInHours { get; set; }
    public double AverageParticipantsPerEvent { get; set; }
    public string? MostPopularEvent { get; set; }
}