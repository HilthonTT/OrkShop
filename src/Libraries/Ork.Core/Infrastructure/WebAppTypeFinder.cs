using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Ork.Core.Infrastructure;

public sealed class WebAppTypeFinder : ITypeFinder
{
    private const string AssemblySkipLoadingPattern =
       "^System|^mscorlib|^Microsoft|^AjaxControlToolkit|^Antlr3|^Autofac|^AutoMapper|^Castle|^ComponentArt|^CppCodeProvider|^DotNetOpenAuth|^EntityFramework|^EPPlus|^FluentValidation|^ImageResizer|^itextsharp|^log4net|^MaxMind|^MbUnit|^MiniProfiler|^Mono.Math|^MvcContrib|^Newtonsoft|^NHibernate|^nunit|^Org.Mentalis|^PerlRegex|^QuickGraph|^Recaptcha|^Remotion|^RestSharp|^Rhino|^Telerik|^Iesi|^TestDriven|^TestFu|^UserAgentStringLibrary|^VJSharpCodeProvider|^WebActivator|^WebDev|^WebGrease";

    private static readonly Dictionary<string, Assembly> _assemblies = new(StringComparer.InvariantCultureIgnoreCase);
    private static readonly List<string> _directoriesToLoadAssemblies = [AppContext.BaseDirectory];

    private static bool _loaded = false;
    private static readonly Lock _locker = new();

    private readonly IOrkFileProvider _fileProvider;

    public WebAppTypeFinder()
    {
        _fileProvider = CommonHelper.DefaultFileProvider;
    }

    static WebAppTypeFinder()
    {
        AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;
    }

    public IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true)
    {
        return FindClassesOfType(typeof(T), onlyConcreteClasses);
    }

    public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true)
    {
        return FindClassesOfType(assignTypeFrom, GetAssemblies(), onlyConcreteClasses);
    }

    public IList<Assembly> GetAssemblies()
    {
        if (!_loaded)
        {
            InitData();
        }

        return _assemblies.Values.ToList();
    }

    public Assembly? GetAssemblyByName(string assemblyFullName)
    {
        if (!_loaded)
        {
            InitData();
        }

        _assemblies.TryGetValue(assemblyFullName, out var assembly);

        if (assembly is not null)
        {
            return assembly;
        }

        var assemblyName = new AssemblyName(assemblyFullName);

        string? key = _assemblies.Keys
            .FirstOrDefault(k => 
                k.StartsWith(assemblyName.Name ?? assemblyFullName.Split(' ')[0], StringComparison.InvariantCultureIgnoreCase));

        return string.IsNullOrEmpty(key) ? null : _assemblies[key];
    }

    private static bool Matches(string assemblyFullName)
    {
        return !Regex.IsMatch(assemblyFullName, AssemblySkipLoadingPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }

    private static void OnAssemblyLoad(object? sender, AssemblyLoadEventArgs args)
    {
        Assembly assembly = args.LoadedAssembly;

        if (assembly.FullName is null)
        {
            return;
        }

        if (_assemblies.ContainsKey(assembly.FullName))
        {
            return;
        }

        if (!Matches(assembly.FullName))
        {
            return;
        }

        _assemblies.TryAdd(assembly.FullName, assembly);
    }

    private static bool DoesTypeImplementOpenGeneric(Type type, Type openGeneric)
    {
        try
        {
            var genericTypeDefinition = openGeneric.GetGenericTypeDefinition();
            return type.GetInterfaces()
                .Any(i => i.IsGenericType && genericTypeDefinition.IsAssignableFrom(i.GetGenericTypeDefinition()));
        }
        catch
        {
            return false;
        }
    }

    private static List<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
    {
        var result = new List<Type>();
        try
        {
            foreach (var assembly in assemblies)
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch
                {
                    continue;
                }
                foreach (var type in types)
                {
                    if (assignTypeFrom.IsAssignableFrom(type) || DoesTypeImplementOpenGeneric(type, assignTypeFrom))
                    {
                        if (!type.IsInterface)
                        {
                            if (onlyConcreteClasses)
                            {
                                if (type.IsClass && !type.IsAbstract)
                                {
                                    result.Add(type);
                                }
                            }
                            else
                            {
                                result.Add(type);
                            }
                        }
                    }
                }
            }
        }
        catch (ReflectionTypeLoadException ex)
        {
            string msg = string.Empty;

            if (ex.LoaderExceptions.Length != 0)
            {
                msg = ex.LoaderExceptions.Where(e => e is not null)
                    .Aggregate(msg, (current, e) => $"{current}{e?.Message + Environment.NewLine}");
            }

            var fail = new Exception(msg, ex);
            Debug.WriteLine(fail.Message, fail);

            throw fail;
        }

        return result;
    }

    private void InitData()
    {
        // Data already loaded
        if (_loaded)
        {
            return;
        }

        // Prevent multi loading data
        lock (_locker)
        {
            // Data can be loaded while we waited
            if (_loaded)
            {
                return;
            }

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (string.IsNullOrWhiteSpace(assembly.FullName) || !Matches(assembly.FullName))
                {
                    continue;
                }

                _assemblies.TryAdd(assembly.FullName, assembly);
            }

            foreach (var directoriesToLoadAssembly in _directoriesToLoadAssemblies)
            {
                if (!_fileProvider.DirectoryExists(directoriesToLoadAssembly))
                {
                    continue;
                }

                foreach (string dllPath in _fileProvider.GetFiles(directoriesToLoadAssembly, "*.dll"))
                {
                    try
                    {
                        var an = AssemblyName.GetAssemblyName(dllPath);

                        if (_assemblies.ContainsKey(an.FullName) || !Matches(an.FullName))
                        {
                            continue;
                        }    

                        Assembly assembly;

                        try
                        {
                            assembly = AppDomain.CurrentDomain.Load(an);
                        }
                        catch
                        {
                            assembly = Assembly.LoadFrom(dllPath);
                        }

                        if (!string.IsNullOrWhiteSpace(assembly.FullName))
                        {
                            _assemblies.TryAdd(assembly.FullName, assembly);
                        }
                    }
                    catch (BadImageFormatException ex)
                    {
                        Trace.TraceError(ex.ToString());
                    }
                }
            }

            _loaded = true;
        }
    }
}
