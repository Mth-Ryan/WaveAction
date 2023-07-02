using WaveActionApi.Repositories;

namespace WaveActionApi.Injections;

public static class RepositoriesInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAccessRepository, AccessRepository>();
        services.AddScoped<IAuthorsRepository, AuthorsRepository>();
        services.AddScoped<IPostsRepository, PostsRepository>();
        services.AddScoped<IThreadsRepository, ThreadsRepository>();

        return services;
    }
}