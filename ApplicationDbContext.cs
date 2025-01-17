using cda_ecf_asp_net.Models;
using Microsoft.EntityFrameworkCore;

namespace cda_ecf_asp_net;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Participant> Participants { get; set; }
    public DbSet<EventParticipant> EventParticipants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventParticipant>()
            .HasKey(ep => new { ep.EventId, ep.ParticipantId });

        modelBuilder.Entity<EventParticipant>()
            .HasOne(ep => ep.Event)
            .WithMany(e => e.EventParticipants)
            .HasForeignKey(ep => ep.EventId);

        modelBuilder.Entity<EventParticipant>()
            .HasOne(ep => ep.Participant)
            .WithMany(p => p.EventParticipants)
            .HasForeignKey(ep => ep.ParticipantId);
    }
}