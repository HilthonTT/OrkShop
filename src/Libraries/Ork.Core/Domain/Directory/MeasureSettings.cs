using Ork.Core.Configuration;

namespace Ork.Core.Domain.Directory;

public sealed class MeasureSettings : ISettings
{
    /// <summary>
    /// Base dimension identifier
    /// </summary>
    public int BaseDimensionId { get; set; }

    /// <summary>
    /// Base weight identifier
    /// </summary>
    public int BaseWeightId { get; set; }
}