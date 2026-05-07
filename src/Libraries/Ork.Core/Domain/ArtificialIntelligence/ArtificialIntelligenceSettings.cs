using Ork.Core.Configuration;

namespace Ork.Core.Domain.ArtificialIntelligence;

public sealed class ArtificialIntelligenceSettings : ISettings
{
    public bool Enabled { get; set; }

    public ArtificialIntelligenceProviderType ProviderType { get; set; }

    public string? GeminiApiKey { get; set; }

    public string? ChatGptApiKey { get; set; }

    public string? DeepSeekApiKey { get; set; }

    public int? RequestTimeout { get; set; }

    public bool AllowProductDescriptionGeneration { get; set; }

    public string ProductDescriptionQuery { get; set; } = string.Empty;

    public bool AllowMetaKeywordsGeneration { get; set; }

    public string? MetaKeywordsQuery { get; set; }

    public bool AllowMetaDescriptionGeneration { get; set; }

    public bool AllowMetaTitleGeneration { get; set; }

    public string? MetaTitleQuery { get; set; }

    public bool LogRequests { get; set; }
}
