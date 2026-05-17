using Ork.Core.ComponentModel;
using Ork.Core.Infrastructure;

namespace Ork.Data.Mapping;

public static class NameCompatibilityManager
{
    public static readonly List<Type> AdditionalNameCompatibilities = [];

    private static readonly Dictionary<Type, string> _tableNames = [];
    private static readonly Dictionary<(Type, string), string> _columnName = [];
    private static readonly List<Type> _loadedFor = [];
    private static bool _isInitialized;
    private static readonly ReaderWriterLockSlim _locker = new();

    public static string GetTableName(Type type)
    {
        if (!_isInitialized)
        {
            Initialize();
        }

        return _tableNames.TryGetValue(type, out var value) ? value : type.Name;
    }

    public static string GetColumnName(Type type, string propertyName)
    {
        if (!_isInitialized)
        {
            Initialize();
        }

        return _columnName.ContainsKey((type, propertyName)) ? _columnName[(type, propertyName)] : propertyName;
    }

    private static void Initialize()
    {
        //perform with locked access to resources
        using (new ReaderWriteLockDisposable(_locker))
        {
            if (_isInitialized)
            {
                return;
            }

            var typeFinder = Singleton<ITypeFinder>.Instance;
            var compatibilities = typeFinder?.FindClassesOfType<INameCompatibility>()
                ?.Select(type => EngineContext.Current.ResolveUnregistered(type) as INameCompatibility).ToList() ?? [];

            compatibilities.AddRange(AdditionalNameCompatibilities
                .Select(type => EngineContext.Current.ResolveUnregistered(type) as INameCompatibility));

            foreach (INameCompatibility? nameCompatibility in compatibilities.Distinct())
            {
                if (nameCompatibility is null || _loadedFor.Contains(nameCompatibility.GetType()))
                {
                    continue;
                }

                _loadedFor.Add(nameCompatibility.GetType());

                foreach (var (key, value) in nameCompatibility.TableNames.Where(tableName =>
                             !_tableNames.ContainsKey(tableName.Key)))
                {
                    _tableNames.Add(key, value);
                }

                foreach (var (key, value) in nameCompatibility.ColumnName.Where(columnName =>
                             !_columnName.ContainsKey(columnName.Key)))
                {
                    _columnName.Add(key, value);
                }
            }

            _isInitialized = true;
        }
    }
}
