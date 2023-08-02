using WaveAction.Application.Dtos.Posts;
using WaveAction.Application.Dtos.Shared;
using WaveAction.Application.Dtos.Threads;
using WaveAction.Domain.Specification;

namespace WaveAction.Application.Interfaces;

public interface IThreadsAppService
{

    public Task<ThreadDto> Get(Guid id);
    public Task<ThreadDto> Get(string titleSlug);
    public Task<PaginatedDataDto<ThreadShortDto>> GetAll(QueryOptions options);
    public Task<PaginatedDataDto<PostShortDto>> GetPosts(Guid id, QueryOptions options);
    public Task<PaginatedDataDto<PostShortDto>> GetPosts(string titleSlug, QueryOptions options);
    public Task<ThreadDto> Create(Guid authorId, ThreadCreateDto input);
    public Task<ThreadDto> Update(Guid authorId, Guid threadId, ThreadCreateDto input);
    public Task Delete(Guid authorId, Guid threadId);
}
