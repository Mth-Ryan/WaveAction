using WaveActionApi.Data;

namespace WaveActionApi.Injections;

public static class DataInjection
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