using Ork.Core.Configuration;

namespace Ork.Core.Domain.Messages;

/// <summary>
/// Email account settings
/// </summary>
public sealed class EmailAccountSettings : ISettings
{
    /// <summary>
    /// Gets or sets a store default email account identifier
    /// </summary>
    public int DefaultEmailAccountId { get; set; }
}