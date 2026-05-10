using Ork.Core.Domain.Common;
using Ork.Core.Domain.Localization;
using Ork.Core.Domain.Security;
using Ork.Core.Domain.Seo;

namespace Ork.Core.Domain.Topics;

public sealed class Topic : BaseEntity, ILocalizedEntity, ISlugSupported, IStoreMappingSupported, IAclSupported, IMetaTagsSupported
{
    public string SystemName { get; set; } = string.Empty;

    public bool IncludeInSitemap { get; set; }

    public int DisplayOrder { get; set; }

    public bool AccessibleWhenStoreClosed { get; set; }

    public bool IsPasswordProtected { get; set; }

    public string Password { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public bool Published { get; set; }

    public int TopicTemplateId { get; set; }

    public bool LimitedToStores { get; set; }

    public bool SubjectToAcl { get; set; }

    public string MetaKeywords { get; set; } = string.Empty;

    public string MetaDescription { get; set; } = string.Empty;

    public string MetaTitle { get; set; } = string.Empty;

    public DateTime? AvailableStartDateTimeUtc { get; set; }

    public DateTime? AvailableEndDateTimeUtc { get; set; }
}
