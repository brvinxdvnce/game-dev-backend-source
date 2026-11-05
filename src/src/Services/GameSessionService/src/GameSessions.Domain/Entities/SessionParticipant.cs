using CSharpFunctionalExtensions;
using GameSessions.Domain.Enums;

namespace GameSessions.Domain.Entities;

public class SessionParticipant
{
    public Guid Id { get; private set; }
    public Guid SessionId { get; private set; }
    public string DisplayName { get; private set; } = default!;
    public Team Team { get; private set; }
    public GameRole Role { get; private set; }
    public DateTime JoinedAt { get; private set; }

    private SessionParticipant() { }

    public static Result<SessionParticipant> Create(Guid sessionId, string displayName, Team team, GameRole role)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            return Result.Failure<SessionParticipant>("Display name cannot be empty.");

        return Result.Success(new SessionParticipant
        {
            Id = Guid.NewGuid(),
            SessionId = sessionId,
            DisplayName = displayName.Trim(),
            Team = team,
            Role = role,
            JoinedAt = DateTime.UtcNow
        });
    }
}