namespace Ork.Core.Domain.Blogs;

public sealed class BlogComment : BaseEntity
{
    public int CustomerId { get; set; }

    public string CommentText { get; set; } = string.Empty;

    public bool IsApproved { get; set; }

    public int StoreId { get; set; }

    public int BlogPostId { get; set; }

    public DateTime CreatedOnUtc { get; set; }
}
