namespace Ork.Core.Events;

/// <summary>
/// Defines a contract for asynchronously publishing events to all registered event handlers.
/// </summary>
/// <remarks>Implementations should ensure that all appropriate handlers for the event type are invoked. The order
/// in which handlers are called is not guaranteed. Thread safety and delivery guarantees may vary by
/// implementation.</remarks>
public interface IEventPublisher
{
    /// <summary>
    /// Publishes an event asynchronously to all registered handlers.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event to publish.</typeparam>
    /// <param name="event">The event instance to publish.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default);
}