using WaveAction.Domain.Models;
using WaveAction.Domain.Specification;

namespace WaveAction.Domain.Interfaces;

public interface IAuthorsRepository
{
    Task<AuthorModel?> GetAuthor(Guid id);
    Task<AuthorModel?> GetAuthor(string username);
    Task<List<AuthorModel>> GetAuthors(QueryOptions options);
    Task<int> GetAuthorsCount(QueryOptions options);
    Task<List<PostModel>> GetAuthorPosts(Guid id, QueryOptions options);
    Task<List<PostModel>> GetAuthorPosts(string userName, QueryOptions options);
    Task<int> GetAuthorPostsCount(Guid id, QueryOptions options);
    Task<int> GetAuthorPostsCount(string userName, QueryOptions options);
    Task<List<ThreadModel>> GetAuthorThreads(Guid id, QueryOptions options);
    Task<List<ThreadModel>> GetAuthorThreads(string userName, QueryOptions options);
    Task<int> GetAuthorThreadsCount(Guid id, QueryOptions options);
    Task<int> GetAuthorThreadsCount(string userName, QueryOptions options);
    Task<int> Save();
}
