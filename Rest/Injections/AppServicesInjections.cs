using WaveAction.Application.Interfaces;
using WaveAction.Application.Services;

namespace WaveAction.Rest.Inejections;

public static class AppServicesInjections
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddTransient<IAuthorsAppService, AuthorsAppService>();
        services.AddTransient<IThreadsAppService, ThreadsAppService>();
        services.AddTransient<IPostsAppService, PostsAppService>();
        return services;
    }
}
