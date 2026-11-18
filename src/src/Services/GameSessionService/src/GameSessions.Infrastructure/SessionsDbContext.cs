using GameSessions.Domain.Entities;
using GameSessions.Infrastructure.Database.Configurations;
using Microsoft.EntityFrameworkCore;

namespace GameSessions.Infrastructure;

public class SessionsDbContext(DbContextOptions<SessionsDbContext> options) : DbContext(options)
{
    public DbSet<GameSession> Sessions { get; set; }
    public DbSet<SessionParticipant> Participants { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SessionConfiguration());
        modelBuilder.ApplyConfiguration(new SessionParticipantConfiguration());
        modelBuilder.ApplyConfiguration(new UserAccountConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}