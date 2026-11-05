using GameSessions.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameSessions.Infrastructure;

public class SessionsDbContext : DbContext
{
    public DbSet<GameSession> Sessions => Set<GameSession>();
    public DbSet<SessionParticipant> Participants => Set<SessionParticipant>();

}