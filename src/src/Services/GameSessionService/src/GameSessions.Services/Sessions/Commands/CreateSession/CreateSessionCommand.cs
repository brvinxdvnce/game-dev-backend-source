using CSharpFunctionalExtensions;
using MediatR;

namespace GameSessions.Services.Sessions.Commands.CreateSession;

public record CreateSessionCommand: IRequest<Result<Guid>>;