namespace Ork.Core.Events;

/// <summary>
/// Event that is published when the application has started.
/// This event can be consumed by plugins or services that need to perform actions
/// once the application is fully initialized.
/// </summary>
public sealed class AppStartedEvent
{
    /// <summary>
    /// Gets the UTC date and time when the application started.
    /// </summary>
    public DateTime StartedAtUtc { get; set; } = DateTime.UtcNow;
}
