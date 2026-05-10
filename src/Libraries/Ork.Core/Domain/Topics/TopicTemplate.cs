namespace Ork.Core.Domain.Topics;

/// <summary>
/// Represents a topic template
/// </summary>
public sealed class TopicTemplate : BaseEntity
{
    /// <summary>
    /// Gets or sets the template name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the view path
    /// </summary>
    public string ViewPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }
}
