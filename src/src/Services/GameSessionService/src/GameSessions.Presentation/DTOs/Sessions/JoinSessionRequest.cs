using GameSessions.Domain.Enums;

namespace GameSessions.Presentation.DTOs.Sessions;

 
public record JoinSessionRequest(
    string DisplayName,
    Team Team,
    GameRole Role);