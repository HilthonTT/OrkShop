namespace Ork.Core.Domain.Blogs;

public sealed class BlogPostTag
{
    public string Name { get; set; } = string.Empty;

    public int BlogPostCount { get; set; }
}
