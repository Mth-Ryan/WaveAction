using AutoMapper;
using Microsoft.Extensions.Logging;
using WaveAction.Application.Dtos.Posts;
using WaveAction.Application.Dtos.Shared;
using WaveAction.Application.Dtos.Threads;
using WaveAction.Domain.Common.Exceptions;
using WaveAction.Domain.Interfaces;
using WaveAction.Domain.Models;
using WaveAction.Domain.Specification;

namespace WaveAction.Application.Services;

public class ThreadsAppService
{
    private readonly ILogger<ThreadsAppService> _logger;
    private readonly IThreadsRepository _repository;
    private readonly IAuthorsRepository _authorsRepo;
    private readonly IMapper _mapper;

    public ThreadsAppService(
        ILogger<ThreadsAppService> logger,
        IThreadsRepository repository,
        IAuthorsRepository authorsRepo,
        IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _authorsRepo = authorsRepo;
        _mapper = mapper;
    }

    public async Task<ThreadDto> Get(Guid id)
    {
        var thread = await _repository.GetThread(id);
        if (thread is null)
            throw new EntityNotFoundException("Post not found");

        return _mapper.Map<ThreadDto>(thread);
    }

    public async Task<ThreadDto> Get(string titleSlug)
    {
        var thread = await _repository.GetThread(titleSlug);
        return _mapper.Map<ThreadDto>(thread);
    }

    public async Task<PaginatedDataDto<ThreadShortDto>> GetAll(QueryOptions options)
    {
        var total = await _repository.GetThreadsCount(options);
        var threads = await _repository.GetThreads(options);
        var data = _mapper.Map<List<ThreadModel>, List<ThreadShortDto>>(threads);

        return new PaginatedDataDto<ThreadShortDto>(options, total, data);
    }

    public async Task<PaginatedDataDto<PostShortDto>> GetPosts(Guid id, QueryOptions options)
    {
        var total = await _repository.GetThreadsCount(options);
        var posts = await _repository.GetThreadPosts(id, options);
        var data = _mapper.Map<List<PostModel>, List<PostShortDto>>(posts);

        return new PaginatedDataDto<PostShortDto>(options, total, data);
    }

    public async Task<PaginatedDataDto<PostShortDto>> GetPosts(string titleSlug, QueryOptions options)
    {
        var total = await _repository.GetThreadsCount(options);
        var posts = await _repository.GetThreadPosts(titleSlug, options);
        var data = _mapper.Map<List<PostModel>, List<PostShortDto>>(posts);

        return new PaginatedDataDto<PostShortDto>(options, total, data);
    }

    public async Task<ThreadDto> Create(Guid authorId, ThreadCreateDto input)
    {
        var author = await _authorsRepo.GetAuthor(authorId);
        if (author is null)
            throw new EntityNotFoundException("Author not found");

        var thread = _mapper.Map<ThreadModel>(input);
        thread.AuthorId = author.Id;

        await _repository.Add(thread);

        thread.Author = author;
        return _mapper.Map<ThreadDto>(thread);
    }

    public async Task<ThreadDto> Update(Guid authorId, Guid threadId, ThreadCreateDto input)
    {
        var thread = await _repository.GetThread(threadId);
        if (thread is null)
            throw new EntityNotFoundException("Post not found");

        var author = await _authorsRepo.GetAuthor(authorId);
        if (author is null)
            throw new EntityNotFoundException("Author not found");

        if (author.Id != thread.AuthorId && !author.Admin)
            throw new ForbidenOperationException("Author forbiden tho change this post");

        _mapper.Map(input, thread);
        thread.UpdatedAt = DateTime.UtcNow;
        await _repository.Save();

        return _mapper.Map<ThreadDto>(thread);
    }

    public async Task Delete(Guid authorId, Guid threadId)
    {
        var thread = await _repository.GetThread(threadId);
        if (thread is null)
            throw new EntityNotFoundException("Post not found");

        var author = await _authorsRepo.GetAuthor(authorId);
        if (author is null)
            throw new EntityNotFoundException("Author not found");

        if (author.Id != thread.AuthorId && !author.Admin)
            throw new ForbidenOperationException("Author forbiden tho change this post");

        await _repository.Delete(thread);
    }
}
