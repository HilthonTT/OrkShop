using Newtonsoft.Json;

namespace Ork.Core.Domain.Common;

public sealed class LicenseTermsInfo
{
    [JsonProperty(PropertyName = "version")]
    public string Version { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "installationDate")]
    public DateTime? InstallationDate { get; set; }

    [JsonProperty(PropertyName = "acceptedLicenseTerms")]
    public bool AcceptedLicenseTerms { get; set; }

    [JsonProperty(PropertyName = "lastCheckDate")]
    public DateTime? LastCheckDate { get; set; }
}