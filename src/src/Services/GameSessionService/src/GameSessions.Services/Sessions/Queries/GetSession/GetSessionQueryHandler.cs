using CSharpFunctionalExtensions;
using GameSessions.Domain.Repositories;
using GameSessions.Services.DTOs.Participant;
using GameSessions.Services.DTOs.Session;
using MediatR;

namespace GameSessions.Services.Sessions.Queries.GetSession;

public sealed class GetSessionQueryHandler
    : IRequestHandler<GetSessionQuery, Result<SessionDto>>
{
    private readonly ISessionRepository _repository;

    public GetSessionQueryHandler(ISessionRepository repository) => _repository = repository;

    public async Task<Result<SessionDto>> Handle(
        GetSessionQuery request,
        CancellationToken cancellationToken)
    {
        var session = await _repository.GetByIdAsync(request.SessionId, cancellationToken);
        if (session is null)
            return Result.Failure<SessionDto>($"Session '{request.SessionId}' was not found.");

        var dto = new SessionDto(
            Id: session.Id,
            State: session.State.ToString(),
            CreatedAt: session.CreatedAt,
            StartedAt: session.StartedAt,
            FinishedAt: session.FinishedAt,
            Participants: session.Participants
                .Select(p => new ParticipantDto(
                    p.Id,
                    p.DisplayName,
                    p.Team.ToString(),
                    p.Role.ToString(),
                    p.JoinedAt))
                .ToList());

        return Result.Success(dto);
    }
}