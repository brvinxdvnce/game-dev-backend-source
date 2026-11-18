namespace DomainEvents;

public abstract class DomainEntity
{
    private List<IEvent> _events = new();
    
    public IReadOnlyCollection<IEvent> Events => this._events;

    public void AddEvent(IEvent @event) { _events.Add(@event); }

    public void ClearEvents() { _events.Clear(); }
}