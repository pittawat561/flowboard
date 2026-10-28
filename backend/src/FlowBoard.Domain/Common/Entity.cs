namespace FlowBoard.Domain.Common;

/// <summary>Base class for every aggregate/entity that is identified by a Guid.</summary>
public abstract class Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.UtcNow;
}
