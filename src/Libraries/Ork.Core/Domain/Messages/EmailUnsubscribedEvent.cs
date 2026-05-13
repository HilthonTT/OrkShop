namespace Ork.Core.Domain.Messages;

/// <summary>
/// Email unsubscribed event
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="subscription">Subscription</param>
public sealed class EmailUnsubscribedEvent(NewsLetterSubscription subscription)
{
    /// <summary>
    /// Subscription
    /// </summary>
    public NewsLetterSubscription Subscription { get; } = subscription;

    public bool Equals(EmailUnsubscribedEvent? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Equals(other.Subscription, Subscription);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != typeof(EmailUnsubscribedEvent))
        {
            return false;
        }

        return Equals((EmailUnsubscribedEvent)obj);
    }

    public override int GetHashCode()
    {
        return Subscription != null ? Subscription.GetHashCode() : 0;
    }
}