using CSharpFunctionalExtensions;
using GameSessions.Domain.Enums;
using GameSessions.Services.DTOs.Session;
using MediatR;

namespace GameSessions.Services.Sessions.Queries.GetSession;

public sealed record GetSessionQuery(
    Guid SessionId) : IRequest<Result<SessionDto>>;