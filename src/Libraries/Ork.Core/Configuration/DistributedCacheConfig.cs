using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Ork.Core.Configuration;

public sealed class DistributedCacheConfig : IConfig
{
    [JsonConverter(typeof(StringEnumConverter))]
    public DistributedCacheType DistributedCacheType { get; set; } = DistributedCacheType.RedisSynchronizedMemory;

    public bool Enabled { get; set; } = false;

    public string ConnectionString { get; set; } = "127.0.0.1:6379,sll=False";

    public string Schema { get; set; } = "dbo";

    public string TableName { get; set; } = "DistributedCache";

    public string InstanceName { get; set; } = "orkCommerce";

    public int PublishIntervalMs { get; set; } = 500;
}
