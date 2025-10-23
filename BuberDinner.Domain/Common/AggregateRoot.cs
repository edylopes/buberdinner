
using BuberDinner.Domain.Common.Interfaces;

namespace BuberDinner.Domain.Common;

public abstract class AggregateRoot : Entity, IHasDomainEvents
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void AddDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();

}
