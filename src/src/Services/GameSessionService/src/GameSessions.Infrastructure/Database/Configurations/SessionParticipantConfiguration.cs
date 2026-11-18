using GameSessions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameSessions.Infrastructure.Database.Configurations;

public class SessionParticipantConfiguration : IEntityTypeConfiguration<SessionParticipant>
{
    public void Configure(EntityTypeBuilder<SessionParticipant> builder)
    {
        builder.ToTable("session_participant");
        
        builder.HasKey(x => new {x.UserId, x.SessionId});
    }
}