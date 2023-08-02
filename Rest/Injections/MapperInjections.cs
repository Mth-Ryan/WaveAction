using AutoMapper;
using WaveAction.Application.Interfaces;
using WaveAction.Infrastructure.Mapper;
using WaveAction.Infrastructure.Services;

namespace WaveAction.Rest.Inejections;

public static class MapperInjections
{
    public static IServiceCollection AddSlugService(this IServiceCollection services)
    {
        services.AddSingleton<ISlugService>(new SlugService());
        return services;
    }

    public static IServiceCollection AddBcryptService(this IServiceCollection services)
    {
        services.AddSingleton<IBcryptService>(new BcryptService());
        return services;
    }

    public static IServiceCollection AddObjectMapper(this IServiceCollection services)
    {
        services.AddSingleton<IMapper>(o =>
            new ObjectMapperFactory(o.GetRequiredService<ISlugService>(), o.GetRequiredService<IBcryptService>())
                .CreateMapper());

        return services;
    }
}
