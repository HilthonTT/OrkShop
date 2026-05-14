namespace Ork.Data;

/// <summary>
/// Represents a data provider manager
/// </summary>
public interface IDataProviderManager
{
    /// <summary>
    /// Gets data provider
    /// </summary>
    IOrkDataProvider DataProvider { get; }
}
