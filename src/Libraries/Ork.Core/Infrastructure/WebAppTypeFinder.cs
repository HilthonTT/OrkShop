using System.Reflection;
using System.Text.RegularExpressions;

namespace Ork.Core.Infrastructure;

public sealed class WebAppTypeFinder : ITypeFinder
{
    private const string AssemblySkipLoadingPattern =
       "^System|^mscorlib|^Microsoft|^AjaxControlToolkit|^Antlr3|^Autofac|^AutoMapper|^Castle|^ComponentArt|^CppCodeProvider|^DotNetOpenAuth|^EntityFramework|^EPPlus|^FluentValidation|^ImageResizer|^itextsharp|^log4net|^MaxMind|^MbUnit|^MiniProfiler|^Mono.Math|^MvcContrib|^Newtonsoft|^NHibernate|^nunit|^Org.Mentalis|^PerlRegex|^QuickGraph|^Recaptcha|^Remotion|^RestSharp|^Rhino|^Telerik|^Iesi|^TestDriven|^TestFu|^UserAgentStringLibrary|^VJSharpCodeProvider|^WebActivator|^WebDev|^WebGrease";

    private static readonly Dictionary<string, Assembly> _assemblies = new(StringComparer.InvariantCultureIgnoreCase);

    private static bool _loaded;
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
        throw new NotImplementedException();
    }

    public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true)
    {
        throw new NotImplementedException();
    }

    public IList<Assembly> GetAssemblies()
    {
        throw new NotImplementedException();
    }

    public Assembly GetAssemblyByName(string assemblyName)
    {
        throw new NotImplementedException();
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
}
