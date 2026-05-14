using FluentMigrator.Runner.Initialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Ork.Core.Configuration;
using System.Configuration;

namespace Ork.Data.Configuration;

internal sealed class DataConfig : IConfig, IConnectionStringAccessor
{
    public string ConnectionString { get; set; } = string.Empty;

    [JsonConverter(typeof(StringEnumConverter))]
    public DataProviderType DataProvider { get; set; } = DataProviderType.PostgreSQL;

    public int? SQLCommandTimeout { get; set; } = null;

    public bool WithNoLock { get; set; } = false;

    public string Collation { get; set; } = string.Empty;

    public string CharacterSet { get; set; } = string.Empty;

    public bool CloseDataContextAfterUse { get; set; } = true;

    public bool BulkCopyWithCheckConstraints { get; set; } = true;

    [JsonIgnore]
    public string Name => nameof(ConfigurationManager.ConnectionStrings);

    public int GetOrder() => 0;
}
