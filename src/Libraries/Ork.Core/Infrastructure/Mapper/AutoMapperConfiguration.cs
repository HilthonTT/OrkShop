using AutoMapper;

namespace Ork.Core.Infrastructure.Mapper;

public static class AutoMapperConfiguration
{
    public static IMapper Mapper { get; private set; } = default!;

    public static MapperConfiguration MapperConfiguration { get; private set; } = default!;

    public static void Init(MapperConfiguration config)
    {
        MapperConfiguration = config;
        Mapper = config.CreateMapper();
    }
}
