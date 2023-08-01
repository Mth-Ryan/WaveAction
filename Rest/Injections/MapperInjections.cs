using AutoMapper;
using Slugify;
using WaveAction.Application.Mapper;

namespace WaveAction.Rest.Inejections;

public static class MapperInjections
{
    public static IServiceCollection AddSlugHelper(this IServiceCollection services)
    {
        services.AddSingleton<ISlugHelper>(new SlugHelper());
        return services;
    }


    public static IServiceCollection AddObjectMapper(this IServiceCollection services)
    {
        services.AddSingleton<IMapper>(o =>
            new ObjectMapperFactory(o.GetRequiredService<ISlugHelper>())
                .CreateMapper());

        return services;
    }
}
