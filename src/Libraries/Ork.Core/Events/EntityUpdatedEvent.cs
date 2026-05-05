namespace Ork.Core.Events;

/// <summary>
/// Initializes a new instance of the <see cref="EntityUpdatedEvent{T}"/> class.
/// </summary>
/// <param name="entity">The entity that was updated.</param>
public sealed class EntityUpdatedEvent<T>(T entity)
    where T : BaseEntity
{
    /// <summary>
    /// Gets the entity that was updated.
    /// </summary>
    public T Entity { get; } = entity;
}