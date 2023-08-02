using WaveAction.Domain.Models;
using WaveAction.Domain.Specification;

namespace WaveAction.Domain.Interfaces;

public interface IPostsRepository
{
    Task<PostModel?> GetPost(Guid id);
    Task<PostModel?> GetPost(string titleSlug);
    Task<List<PostModel>> GetPosts(QueryOptions options);
    Task<int> GetPostsCount(QueryOptions options);
    Task<int> Save();
    Task<int> Add(PostModel post);
    Task<int> Delete(PostModel post);
}
