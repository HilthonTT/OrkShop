namespace Ork.Core.Domain.Blogs;

/// <summary>
/// Blog post comment approved event
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="blogComment">Blog comment</param>
public sealed class BlogCommentApprovedEvent(BlogComment blogComment)
{
    /// <summary>
    /// Blog post comment
    /// </summary>
    public BlogComment BlogComment { get; } = blogComment;
}