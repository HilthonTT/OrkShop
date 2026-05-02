namespace Ork.Core.Caching;

/// <summary>
/// Represents default values related to caching entities
/// </summary>
public static partial class OrkEntityCacheDefaults<TEntity> where TEntity : BaseEntity
{
    /// <summary>
    /// Gets an entity type name used in cache keys
    /// </summary>
    public static string EntityTypeName => typeof(TEntity).Name.ToLowerInvariant();

    /// <summary>
    /// Gets a key for caching entity by identifier
    /// </summary>
    /// <remarks>
    /// {0} : entity id
    /// </remarks>
    public static CacheKey ByIdCacheKey => new($"Ork.{EntityTypeName}.byid.{{0}}");

    /// <summary>
    /// Gets a key for caching entities by identifiers
    /// </summary>
    /// <remarks>
    /// {0} : entity ids
    /// </remarks>
    public static CacheKey ByIdsCacheKey => new($"Ork.{EntityTypeName}.byids.{{0}}");

    /// <summary>
    /// Gets a key for caching all entities
    /// </summary>
    public static CacheKey AllCacheKey => new($"Ork.{EntityTypeName}.all.");

    /// <summary>
    /// Gets a key pattern to clear cache
    /// </summary>
    public static string Prefix => $"Ork.{EntityTypeName}.";

    /// <summary>
    /// Gets a key pattern to clear cache
    /// </summary>
    public static string ByIdPrefix => $"Ork.{EntityTypeName}.byid.";

    /// <summary>
    /// Gets a key pattern to clear cache
    /// </summary>
    public static string ByIdsPrefix => $"Ork.{EntityTypeName}.byids.";

    /// <summary>
    /// Gets a key pattern to clear cache
    /// </summary>
    public static string AllPrefix => $"Ork.{EntityTypeName}.all.";
}
