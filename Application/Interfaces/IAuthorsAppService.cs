using WaveAction.Application.Dtos.Author;
using WaveAction.Application.Dtos.Posts;
using WaveAction.Application.Dtos.Shared;
using WaveAction.Application.Dtos.Threads;
using WaveAction.Domain.Specification;

namespace WaveAction.Application.Interfaces;

public interface IAuthorsAppService
{
    public Task<AuthorDto> Get(Guid id);
    public Task<PaginatedDataDto<AuthorShortDto>> GetAll(QueryOptions options);
    public Task<PaginatedDataDto<PostShortDto>> GetPosts(string userName, QueryOptions options);
    public Task<PaginatedDataDto<PostShortDto>> GetPosts(Guid id, QueryOptions options);
    public Task<PaginatedDataDto<ThreadDto>> GetThreads(string userName, QueryOptions options);
    public Task<PaginatedDataDto<ThreadDto>> GetThreads(Guid id, QueryOptions options);
    public Task<AuthorDto> UpdateProfile(Guid id, AuthorProfileDto profile);
}
