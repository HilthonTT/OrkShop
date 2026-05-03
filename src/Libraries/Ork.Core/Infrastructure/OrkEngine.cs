using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ork.Core.Infrastructure.Mapper;
using System.Reflection;

namespace Ork.Core.Infrastructure;

public sealed class OrkEngine : IEngine
{
    public IServiceProvider? ServiceProvider { get; set; }

    public void ConfigureRequestPipeline(IApplicationBuilder app)
    {
        ServiceProvider = app.ApplicationServices;

        ITypeFinder? typeFinder = Singleton<ITypeFinder>.Instance;
        var startupConfigurations = typeFinder?.FindClassesOfType<IOrkStartup>() ?? [];

        var instances = startupConfigurations
            .Select(s => (IOrkStartup?)Activator.CreateInstance(s))
            .Where(s => s is not null)
            .OrderBy(s => s!.Order);

        foreach (var instance in instances)
        {
            instance?.Configure(app);
        }
    }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEngine>(this);

        var typeFinder = Singleton<ITypeFinder>.Instance;
        var startupConfigurations = typeFinder?.FindClassesOfType<IOrkStartup>() ?? [];

        var instances = startupConfigurations
            .Select(s => (IOrkStartup?)Activator.CreateInstance(s))
            .Where(s => s is not null)
            .OrderBy(s => s!.Order);

        foreach (var instance in instances)
        {
            instance?.ConfigureServices(services, configuration);
        }

        AddAutoMapper(services);

        services.AddSingleton(services);

        RunStartupTasks();

        //resolve assemblies here. otherwise, plugins can throw an exception when rendering views
        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainAssemblyResolve;
    }

    public T? Resolve<T>(IServiceScope? scope = null) where T : class
    {
        return (T?)Resolve(typeof(T), scope);
    }

    public object? Resolve(Type type, IServiceScope? scope = null)
    {
        return GetServiceProvider(scope).GetService(type);
    }

    public IEnumerable<T> ResolveAll<T>()
    {
        return(IEnumerable<T>)GetServiceProvider().GetServices(typeof(T));
    }

    public object? ResolveUnregistered(Type type)
    {
        Exception? innerException = null;

        foreach (var constructor in type.GetConstructors())
        {
            try
            {
                // try to resolve constructor parameters
                var parameters = constructor.GetParameters().Select(parameter =>
                {
                    var service = Resolve(parameter.ParameterType) ?? throw new OrkException("Unknown dependency");
                    return service;
                });

                // all is ok, so create instance
                return Activator.CreateInstance(type, parameters.ToArray());
            }
            catch (Exception ex)
            {
                innerException = ex;
            }
        }

        if (innerException is not null)
        {
            throw new OrkException("No constructor was found that had all the dependencies satisfied.", innerException);
        }
        else
        {
            throw new OrkException("No constructor was found that had all the dependencies satisfied.");
        }
    }

    private IServiceProvider GetServiceProvider(IServiceScope? scope = null)
    {
        if (scope is not null)
        {
            return scope.ServiceProvider;
        }

        IHttpContextAccessor? httpContextAccessor = ServiceProvider?.GetService<IHttpContextAccessor>();
        HttpContext? httpContext = httpContextAccessor?.HttpContext;
        return httpContext?.RequestServices ?? ServiceProvider 
            ?? throw new InvalidOperationException("Service provider is not available.");
    }

    private static void RunStartupTasks()
    {
        ITypeFinder? typeFinder = Singleton<ITypeFinder>.Instance;
        if (typeFinder is null)
        {
            return;
        }

        IEnumerable<Type> startupTaskTypes = typeFinder.FindClassesOfType<IStartupTask>();

        var instances = startupTaskTypes
            .Select(st => (IStartupTask?)Activator.CreateInstance(st))
            .Where(st => st is not null)
            .OrderBy(st => st!.Order);

        foreach (var instance in instances)
        {
            instance?.Execute();
        }
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        ITypeFinder? typeFinder = Singleton<ITypeFinder>.Instance;
        if (typeFinder is null)
        {
            return;
        }

        IEnumerable<Type> mapperConfigurations = typeFinder.FindClassesOfType<IOrderedMapperProfile>();

        var instances = mapperConfigurations
            .Select(configuration => (IOrderedMapperProfile?)Activator.CreateInstance(configuration))
            .Where(instance => instance is not null)
            .OrderBy(instance => instance!.Order)
            .ToList();

        services.AddAutoMapper(cfg =>
        {
            foreach (var instance in instances)
            {
                // Pass the profile instance directly — AddProfile(Type) was removed
                if (instance is AutoMapper.Profile profile)
                {
                    cfg.AddProfile(profile);
                }
            }
        });
    }

    private static Assembly? CurrentDomainAssemblyResolve(object? sender, ResolveEventArgs args)
    {
        var assemblyName = new AssemblyName(args.Name);

        Assembly? assembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == assemblyName.Name);

        if (assembly is not null)
        {
            return assembly;
        }

        ITypeFinder? typeFinder = Singleton<ITypeFinder>.Instance;

        return typeFinder?.GetAssemblyByName(assemblyName.Name ?? args.Name);
    }
}
