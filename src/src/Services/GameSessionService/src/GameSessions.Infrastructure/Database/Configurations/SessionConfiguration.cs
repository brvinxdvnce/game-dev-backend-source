using GameSessions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameSessions.Infrastructure.Database.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<GameSession>
{
    public void Configure(EntityTypeBuilder<GameSession> builder)
    {
        builder.ToTable("session");

        builder.HasKey(s => s.Id);
    }
}