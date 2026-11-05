using GameSessions.Domain.Entities;

namespace GameSessions.Domain.Repositories;

public interface ISessionRepository
{
    Task<GameSession?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(GameSession session, CancellationToken ct = default);
    Task UpdateAsync(GameSession session, CancellationToken ct = default);
}