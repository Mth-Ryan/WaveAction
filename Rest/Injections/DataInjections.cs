using WaveAction.Infrastructure.Contexts;

namespace WaveAction.Rest.Inejections;

public static class DataInjections
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfigurationRoot config)
    {
        services.AddDbContext<BlogContext>();
        services.AddStackExchangeRedisCache(o =>
        {
            o.Configuration = config.GetConnectionString("Redis");
            o.InstanceName = "WaveActionCache";
        });

        return services;
    }
}
