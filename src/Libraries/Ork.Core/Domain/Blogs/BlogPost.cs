using Ork.Core.Domain.Common;
using Ork.Core.Domain.Localization;
using Ork.Core.Domain.Seo;

namespace Ork.Core.Domain.Blogs;

public sealed class BlogPost : BaseEntity, ISlugSupported, IStoreMappingSupported, IMetaTagsSupported
{
    public int LanguageId { get; set; }

    public bool IncludeInSitemap { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public string BodyOverview { get; set; } = string.Empty;

    public bool AllowComments { get; set; }

    public string Tags { get; set; } = string.Empty;

    public string MetaKeywords { get; set; } = string.Empty;

    public string MetaDescription { get; set; } = string.Empty;

    public string MetaTitle { get; set; } = string.Empty;

    public bool LimitedToStores { get; set; }

    public DateTime CreatedOnUtc { get; set; }
}
