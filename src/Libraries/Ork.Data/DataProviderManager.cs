using Ork.Core;
using Ork.Core.Infrastructure;
using Ork.Data.Configuration;

namespace Ork.Data;

internal sealed class DataProviderManager : IDataProviderManager
{
    public static IOrkDataProvider GetDataProvider(DataProviderType? dataProviderType)
    {
        return dataProviderType switch
        {
            // TODO: Implement providers
            //DataProviderType.SqlServer => new MsSqlNopDataProvider(),
            //DataProviderType.MySql => new MySqlNopDataProvider(),
            //DataProviderType.PostgreSQL => new PostgreSqlDataProvider(),
            _ => throw new OrkException($"Not supported data provider name: '{dataProviderType}'"),
        };
    }

    public IOrkDataProvider DataProvider
    {
        get
        {
            DataProviderType? dataProviderType = Singleton<DataConfig>.Instance?.DataProvider;
            return GetDataProvider(dataProviderType);
        }
    }
}
