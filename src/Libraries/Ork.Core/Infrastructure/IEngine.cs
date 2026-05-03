using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ork.Core.Infrastructure;

public interface IEngine
{
    void ConfigureServices(IServiceCollection services, IConfiguration configuration);

    void ConfigureRequestPipeline(IApplicationBuilder app);

    T? Resolve<T>(IServiceScope? scope = null) where T : class;

    object? Resolve(Type type, IServiceScope? scope = null);

    IEnumerable<T> ResolveAll<T>();

    object? ResolveUnregistered(Type type);
}