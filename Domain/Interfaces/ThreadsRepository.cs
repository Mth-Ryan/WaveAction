using WaveAction.Domain.Models;
using WaveAction.Domain.Specification;

namespace WaveAction.Domain.Interfaces;

public interface IThreadsRepository
{
    Task<ThreadModel?> GetThread(Guid id);
    Task<ThreadModel?> GetThread(string titleSlug);
    Task<List<ThreadModel>> GetThreads(QueryOptions options);
    Task<List<PostModel>> GetThreadPosts(Guid id, QueryOptions options);
    Task<List<PostModel>> GetThreadPosts(string titleSlug, QueryOptions options);
    Task<int> GetThreadPostsCount(Guid id, QueryOptions options);
    Task<int> GetThreadPostsCount(string titleSlug, QueryOptions options);
    Task<int> GetThreadsCount(QueryOptions options);
    Task<int> Save();
    Task<int> Add(ThreadModel thread);
    Task<int> Delete(ThreadModel thread);
}
