namespace Ork.Core.Events;

/// <summary>
/// Initializes a new instance of the <see cref="EntityInsertedEvent{T}"/> class.
/// </summary>
/// <param name="entity">The entity that was inserted.</param>
public sealed class EntityInsertedEvent<T>(T entity)
    where T : BaseEntity
{
    /// <summary>
    /// Gets the entity that was inserted.
    /// </summary>
    public T Entity { get; } = entity;
}
