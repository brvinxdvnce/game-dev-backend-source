using CSharpFunctionalExtensions;
using GameSessions.Domain.Enums;

namespace GameSessions.Domain.Entities;

public class GameSession
{
    public Guid Id { get; private set; }
    public SessionState State { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? FinishedAt { get; private set; }

    private readonly List<SessionParticipant> _participants = new();
    public IReadOnlyCollection<SessionParticipant> Participants => _participants.AsReadOnly();

    private GameSession() { }

    public static GameSession Create()
    {
        return new GameSession
        {
            Id = Guid.NewGuid(),
            State = SessionState.Created,
            CreatedAt = DateTime.UtcNow
        };
    }

    public Result<SessionParticipant> AddParticipant(string displayName, Team team, GameRole role)
    {
        if (State != SessionState.Created && State != SessionState.InProgress)
            return Result.Failure<SessionParticipant>("Cannot join a finished or cancelled session.");

        if (_participants.Any(p => p.DisplayName == displayName))
            return Result.Failure<SessionParticipant>("SessionParticipant with the same name already exists.");

        var participantResult = SessionParticipant.Create(Id, displayName, team, role);
        if (participantResult.IsFailure)
            return Result.Failure<SessionParticipant>(participantResult.Error);

        _participants.Add(participantResult.Value);
        return Result.Success(participantResult.Value);
    }

    public UnitResult<string> Start()
    {
        if (State != SessionState.Created)
            return UnitResult.Failure("Only a created session can be started.");

        State = SessionState.InProgress;
        StartedAt = DateTime.UtcNow;
        return UnitResult.Success<string>();
    }

    public UnitResult<string> Finish()
    {
        if (State != SessionState.InProgress)
            return UnitResult.Failure("Only an in‑progress session can be finished.");

        State = SessionState.Finished;
        FinishedAt = DateTime.UtcNow;
        return UnitResult.Success<string>();
    }
}