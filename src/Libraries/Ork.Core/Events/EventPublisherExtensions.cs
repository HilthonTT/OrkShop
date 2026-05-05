namespace Ork.Core.Events;

public static class EventPublisherExtensions
{
    public static async Task EntityInsertedAsync<T>(
        this IEventPublisher eventPublisher, T entity, 
        CancellationToken cancellationToken = default) 
        where T : BaseEntity
    {
        await eventPublisher.PublishAsync(new EntityInsertedEvent<T>(entity), cancellationToken);
    }

    public static async Task EntityUpdatedAsync<T>(
        this IEventPublisher eventPublisher, T entity,
        CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        await eventPublisher.PublishAsync(new EntityUpdatedEvent<T>(entity), cancellationToken);
    }

    public static async Task EntityDeletedAsync<T>(
        this IEventPublisher eventPublisher, T entity,
        CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        await eventPublisher.PublishAsync(new EntityDeletedEvent<T>(entity), cancellationToken);
    }
}
