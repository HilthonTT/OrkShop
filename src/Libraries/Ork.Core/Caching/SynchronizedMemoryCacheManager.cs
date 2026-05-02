using Microsoft.Extensions.Caching.Memory;
using Ork.Core.Configuration;

namespace Ork.Core.Caching;

/// <summary>
/// Represents a memory cache manager with distributed synchronization
/// </summary>
/// <remarks>
/// This class should be registered on IoC as singleton instance
/// </remarks>
public sealed class SynchronizedMemoryCacheManager(
    AppSettings appSettings, 
    IMemoryCache memoryCache, 
    ICacheKeyManager cacheKeyManager) : MemoryCacheManager(appSettings, memoryCache, cacheKeyManager)
{
}
