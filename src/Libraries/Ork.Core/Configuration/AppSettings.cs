using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace Ork.Core.Configuration;

public sealed class AppSettings(IList<IConfig>? configurations = null)
{
    private readonly Dictionary<Type, IConfig> _configurations = configurations
                              ?.OrderBy(config => config.GetOrder())
                              ?.ToDictionary(config => config.GetType(), config => config)
                          ?? [];

    public TConfig Get<TConfig>()
        where TConfig : class, IConfig
    {
        if (!_configurations.TryGetValue(typeof(TConfig), out IConfig? config))
        {
            throw new OrkException($"No configuration with type '{typeof(TConfig)}' found");
        }

        return (TConfig)config;
    }

    public void Update(IList<IConfig> configurations)
    {
        foreach (IConfig config in configurations)
        {
            _configurations[config.GetType()] = config;
        }
    }

    [JsonExtensionData]
    public Dictionary<string, JToken> Configuration { get; set; } = new();
}
