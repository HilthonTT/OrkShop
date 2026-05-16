using LinqToDB.Data;

namespace Ork.Data.DataProviders;

public abstract class BaseDataProvider
{
    protected virtual BulkCopyOptions CreateBulkCopyOptions()
    {
        return new BulkCopyOptions
        {
            
        };
    }
}
