using GameSessions.Services.DTOs.Participant;

namespace GameSessions.Services.DTOs.Session;

public sealed record SessionDto(
    Guid Id,
    string State,
    DateTime CreatedAt,
    DateTime? StartedAt,
    DateTime? FinishedAt,
    IReadOnlyList<ParticipantDto> Participants);