using CSharpFunctionalExtensions;
using GameSessions.Domain.Enums;
using MediatR;

namespace GameSessions.Services.Sessions.Commands.JoinSession;

public sealed record JoinSessionCommand(
    Guid SessionId,
    string DisplayName,
    Team Team,
    GameRole Role) : IRequest<Result<Guid>>;