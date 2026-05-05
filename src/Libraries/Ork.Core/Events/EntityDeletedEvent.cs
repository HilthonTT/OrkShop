namespace Ork.Core.Events;

/// <summary>
/// Initializes a new instance of the <see cref="EntityDeletedEvent{T}"/> class.
/// </summary>
/// <param name="entity">The entity that was deleted.</param>
public sealed class EntityDeletedEvent<T>(T entity)
    where T : BaseEntity
{
    /// <summary>
    /// Gets the entity that was deleted.
    /// </summary>
    public T Entity { get; } = entity;
}
