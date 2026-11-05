namespace GameSessions.Services.DTOs.Participant;

public sealed record ParticipantDto(
    Guid Id,
    string DisplayName,
    string Team,
    string Role,
    DateTime JoinedAt);