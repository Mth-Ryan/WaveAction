using AutoMapper;
using Microsoft.Extensions.Logging;
using WaveAction.Application.Dtos.Posts;
using WaveAction.Application.Dtos.Shared;
using WaveAction.Application.Interfaces;
using WaveAction.Domain.Common.Exceptions;
using WaveAction.Domain.Interfaces;
using WaveAction.Domain.Models;
using WaveAction.Domain.Specification;

namespace WaveAction.Application.Services;

public class PostsAppService : IPostsAppService
{
    private readonly ILogger<PostsAppService> _logger;
    private readonly IPostsRepository _repository;
    private readonly IAuthorsRepository _authorsRepo;
    private readonly IMapper _mapper;

    public PostsAppService(
        ILogger<PostsAppService> logger,
        IPostsRepository repository,
        IAuthorsRepository authorsRepo,
        IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _authorsRepo = authorsRepo;
        _mapper = mapper;
    }

    public async Task<PostDto> Get(Guid id)
    {
        var post = await _repository.GetPost(id);
        if (post is null)
            throw new EntityNotFoundException("Post not found");

        return _mapper.Map<PostDto>(post);
    }

    public async Task<PostDto> Get(string titleSlug)
    {
        var post = await _repository.GetPost(titleSlug);
        return _mapper.Map<PostDto>(post);
    }

    public async Task<PaginatedDataDto<PostShortDto>> GetAll(QueryOptions options)
    {
        var total = await _repository.GetPostsCount(options);
        var posts = await _repository.GetPosts(options);
        var data = _mapper.Map<List<PostModel>, List<PostShortDto>>(posts);

        return new PaginatedDataDto<PostShortDto>(options, total, data);
    }

    public async Task<PostDto> Create(Guid authorId, PostCreateDto input)
    {
        var author = await _authorsRepo.GetAuthor(authorId);
        if (author is null)
            throw new EntityNotFoundException("Author not found");

        var post = _mapper.Map<PostModel>(input);
        post.AuthorId = author.Id;

        await _repository.Add(post);

        post.Author = author;
        return _mapper.Map<PostDto>(post);
    }

    public async Task<PostDto> Update(Guid authorId, Guid postId, PostCreateDto input)
    {
        var post = await _repository.GetPost(postId);
        if (post is null)
            throw new EntityNotFoundException("Post not found");

        var author = await _authorsRepo.GetAuthor(authorId);
        if (author is null)
            throw new EntityNotFoundException("Author not found");

        if (author.Id != post.AuthorId && !author.Admin)
            throw new ForbidenOperationException("Author forbiden tho change this post");

        _mapper.Map(input, post);
        post.UpdatedAt = DateTime.UtcNow;
        await _repository.Save();

        return _mapper.Map<PostDto>(post);
    }

    public async Task Delete(Guid authorId, Guid postId)
    {
        var post = await _repository.GetPost(postId);
        if (post is null)
            throw new EntityNotFoundException("Post not found");

        var author = await _authorsRepo.GetAuthor(authorId);
        if (author is null)
            throw new EntityNotFoundException("Author not found");

        if (author.Id != post.AuthorId && !author.Admin)
            throw new ForbidenOperationException("Author forbiden tho change this post");

        await _repository.Delete(post);
    }
}
