


using BuberDinner.Domain.Common.Interfaces;

namespace BuberDinner.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; protected set; }

    protected readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();

    public static bool operator ==(Entity a, Entity b)
    {
        if (a is null && b is null)
            return true;
        if (a is null || b is null)
            return false;
        return a.Equals(b);
    }

    public static bool operator !=(Entity a, Entity b) => !(a == b);
    public override int GetHashCode() => Id.GetHashCode();

}
