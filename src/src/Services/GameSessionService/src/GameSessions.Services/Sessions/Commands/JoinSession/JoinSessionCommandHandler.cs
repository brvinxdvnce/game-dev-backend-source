using CSharpFunctionalExtensions;
using GameSessions.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GameSessions.Services.Sessions.Commands.JoinSession;

public sealed class JoinSessionCommandHandler
    : IRequestHandler<JoinSessionCommand, Result<Guid>>
{
    private readonly ISessionRepository _repository;
    private readonly ILogger<JoinSessionCommandHandler> _logger;

    public JoinSessionCommandHandler(
        ISessionRepository repository,
        ILogger<JoinSessionCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(
        JoinSessionCommand request,
        CancellationToken cancellationToken)
    {
        var session = await _repository.GetByIdAsync(request.SessionId, cancellationToken);
        if (session is null)
            return Result.Failure<Guid>($"Session '{request.SessionId}' was not found.");

        // Доменный метод возвращает Result<Participant>; сам решает, можно ли добавить.
        var participantResult = session.AddParticipant(
            request.DisplayName,
            request.Team,
            request.Role);

        if (participantResult.IsFailure)
        {
            _logger.LogWarning(
                "Failed to add participant '{DisplayName}' to session {SessionId}: {Reason}",
                request.DisplayName, request.SessionId, participantResult.Error);

            return Result.Failure<Guid>(participantResult.Error);
        }

        await _repository.UpdateAsync(session, cancellationToken);

        _logger.LogInformation(
            "Participant {ParticipantId} ('{DisplayName}') joined session {SessionId} as {Team}/{Role}",
            participantResult.Value.Id,
            participantResult.Value.DisplayName,
            request.SessionId,
            participantResult.Value.Team,
            participantResult.Value.Role);

        return Result.Success(participantResult.Value.Id);
    }
}