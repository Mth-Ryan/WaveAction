using AutoMapper;
using Microsoft.Extensions.Logging;
using WaveAction.Application.Dtos.Author;
using WaveAction.Domain.Interfaces;
using WaveAction.Domain.Common.Exceptions;
using WaveAction.Domain.Specification;
using WaveAction.Application.Dtos.Shared;
using WaveAction.Domain.Models;
using WaveAction.Application.Dtos.Posts;
using WaveAction.Application.Dtos.Threads;
using WaveAction.Application.Interfaces;

namespace WaveAction.Application.Services;

public class AuthorsAppService : IAuthorsAppService
{
    private readonly ILogger<AuthorsAppService> _logger;
    private readonly IAuthorsRepository _repository;
    private readonly IMapper _mapper;

    public AuthorsAppService(
        ILogger<AuthorsAppService> logger,
        IAuthorsRepository repository,
        IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AuthorDto> Get(Guid id)
    {
        var author = await _repository.GetAuthor(id);
        if (author is null)
            throw new EntityNotFoundException("User not found");

        return _mapper.Map<AuthorDto>(author);
    }

    public async Task<PaginatedDataDto<AuthorShortDto>> GetAll(QueryOptions options)
    {
        var total = await _repository.GetAuthorsCount(options);
        var authors = await _repository.GetAuthors(options);
        var data = _mapper.Map<List<AuthorModel>, List<AuthorShortDto>>(authors);

        return new PaginatedDataDto<AuthorShortDto>(options, total, data);
    }

    public async Task<PaginatedDataDto<PostShortDto>> GetPosts(string userName, QueryOptions options)
    {
        var total = await _repository.GetAuthorPostsCount(userName, options);
        var posts = await _repository.GetAuthorPosts(userName, options);
        var data = _mapper.Map<List<PostModel>, List<PostShortDto>>(posts);

        return new PaginatedDataDto<PostShortDto>(options, total, data);
    }

    public async Task<PaginatedDataDto<PostShortDto>> GetPosts(Guid id, QueryOptions options)
    {
        var total = await _repository.GetAuthorPostsCount(id, options);
        var posts = await _repository.GetAuthorPosts(id, options);
        var data = _mapper.Map<List<PostModel>, List<PostShortDto>>(posts);

        return new PaginatedDataDto<PostShortDto>(options, total, data);
    }

    public async Task<PaginatedDataDto<ThreadDto>> GetThreads(string userName, QueryOptions options)
    {
        var total = await _repository.GetAuthorThreadsCount(userName, options);
        var threads = await _repository.GetAuthorThreads(userName, options);
        var data = _mapper.Map<List<ThreadModel>, List<ThreadDto>>(threads);

        return new PaginatedDataDto<ThreadDto>(options, total, data);
    }

    public async Task<PaginatedDataDto<ThreadDto>> GetThreads(Guid id, QueryOptions options)
    {
        var total = await _repository.GetAuthorThreadsCount(id, options);
        var threads = await _repository.GetAuthorThreads(id, options);
        var data = _mapper.Map<List<ThreadModel>, List<ThreadDto>>(threads);

        return new PaginatedDataDto<ThreadDto>(options, total, data);
    }

    public async Task<AuthorDto> UpdateProfile(Guid id, AuthorProfileDto profile)
    {
        var author = await _repository.GetAuthor(id);
        if (author is null)
            throw new EntityNotFoundException("User not found");

        _mapper.Map(profile, author.Profile);
        await _repository.Save();

        return _mapper.Map<AuthorDto>(author);
    }
}
