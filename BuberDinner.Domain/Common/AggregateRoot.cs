
namespace BuberDinner.Domain.Common;

public abstract class AggregateRoot : Entity
{
    // Você pode adicionar propriedades relacionadas a versionamento ou controle de persistência aqui.
    // Exemplo:
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; protected set; }

    protected void MarkUpdated() => UpdatedAt = DateTime.UtcNow;
}
