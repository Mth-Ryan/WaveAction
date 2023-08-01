using WaveAction.Application.Dtos.Posts;
using WaveAction.Application.Dtos.Shared;
using WaveAction.Domain.Specification;

namespace WaveAction.Application.Interfaces;

public interface IPostsAppService
{
    public Task<PostDto> Get(Guid id);
    public Task<PostDto> Get(string titleSlug);
    public Task<PaginatedDataDto<PostShortDto>> GetAll(QueryOptions options);
    public Task<PostDto> Create(Guid authorId, PostCreateDto input);
    public Task<PostDto> Update(Guid authorId, Guid postId, PostCreateDto input);
    public Task Delete(Guid authorId, Guid postId);
}
