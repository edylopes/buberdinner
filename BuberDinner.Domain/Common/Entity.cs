
namespace BuberDinner.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; protected set; }
    protected void MarkUpdated() => UpdatedAt = DateTime.UtcNow;

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
