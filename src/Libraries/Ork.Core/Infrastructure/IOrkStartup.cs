using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ork.Core.Infrastructure;

public interface IOrkStartup
{
    void ConfigureServices(IServiceCollection services, IConfiguration configuration);

    void Configure(IApplicationBuilder application);

    int Order { get; }
}