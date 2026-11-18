using System.Runtime.CompilerServices;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace DomainEvents;
public class DomainEventsInterceptor(IPublishEndpoint publishEndpoint) : SaveChangesInterceptor
{
    private List<DomainEntity> _pendingEntities = [];

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken ct = default)
    {
        if (eventData.Context is not null)
        {
            var logger = eventData.Context.GetService<ILogger<DomainEventsInterceptor>>();
            logger.LogInformation("Publishing domain events");
        
            _pendingEntities = eventData.Context.ChangeTracker
                .Entries<DomainEntity>()
                .Select(e => e.Entity)
                .Where(e => e.Events.Any())
                .ToList();
        }

        return await base.SavingChangesAsync(eventData, result, ct);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData, 
        int result,
        CancellationToken ct = default)
    {
        foreach (var entity in _pendingEntities)
        {
            foreach (var domainEvent in entity.Events)
                await publishEndpoint.Publish(domainEvent, domainEvent.GetType(), ct);
        
            entity.ClearEvents();
        }
        
        return await base.SavedChangesAsync(eventData, result, ct);
    }
}