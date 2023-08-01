using WaveAction.Domain.Interfaces;
using WaveAction.Infrastructure.Repositories;

namespace WaveAction.Rest.Inejections;

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
