using CSharpFunctionalExtensions;
using GameSessions.Domain.Entities;
using GameSessions.Domain.Repositories;
using MediatR;

namespace GameSessions.Services.Sessions.Commands.CreateSession;

public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, Result<Guid>>
{
    private readonly ISessionRepository _repository;

    public CreateSessionCommandHandler(ISessionRepository repository) => _repository = repository;

    public async Task<Result<Guid>> Handle(CreateSessionCommand request, CancellationToken ct)
    {
        var session = GameSession.Create();
        await _repository.AddAsync(session, ct);
        return Result.Success(session.Id);
    }
}