using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaveActionApi.Dtos.Posts;
using WaveActionApi.Dtos.Shared;
using WaveActionApi.Models;
using WaveActionApi.Repositories;
using WaveActionApi.Services;

namespace WaveActionApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly ILogger<PostsController> _logger;
    private readonly IPostsRepository _repository;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwt;

    public PostsController(
        ILogger<PostsController> logger,
        IPostsRepository repository,
        IMapper mapper,
        IJwtService jwt)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
        _jwt = jwt;
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}", Name = "Posts Get")]
    [ProducesResponseType(typeof(PostDto), 200)]
    public async Task<IActionResult> Get(Guid id)
    {
        var post = await _repository.GetPost(id);

        return post is null
            ? BadRequest("Unable to find a post with the given Id")
            : Ok(_mapper.Map<PostDto>(post));
    }

    [AllowAnonymous]
    [HttpGet("{titleSlug}", Name = "Posts Get From Title Slug")]
    [ProducesResponseType(typeof(PostDto), 200)]
    public async Task<IActionResult> Get(string titleSlug)
    {
        var post = await _repository.GetPost(titleSlug);

        return post is null
            ? BadRequest("Unable to find a post with the given Title Slug")
            : Ok(_mapper.Map<PostDto>(post));
    }


    [AllowAnonymous]
    [HttpGet(Name = "Posts Get All")]
    [ProducesResponseType(typeof(PaginatedDataDto<PostShortDto>), 200)]
    public async Task<IActionResult> Get([FromQuery] QueryOptions options)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var total = await _repository.GetPostsCount(options);
        var posts = await _repository.GetPosts(options);
        var data = _mapper.Map<List<PostModel>, List<PostShortDto>>(posts);

        return Ok(new PaginatedDataDto<PostShortDto>
        {
            Search = options.SimpleSearch,
            ItemsTotalCount = (uint)total,
            Page = options.Page,
            PageSize = options.PageSize,
            Order = options.OrderBy,
            Data = data!
        });
    }

    [HttpPost(Name = "Posts Create")]
    [ProducesResponseType(typeof(PostDto), 200)]
    public async Task<IActionResult> Create([FromBody] PostCreateDto postCreateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var author = await _jwt.GetAuthorFromRequest();
        if (author is null) return BadRequest("Unable to find the author");

        var post = _mapper.Map<PostModel>(postCreateDto);
        post.AuthorId = author.Id;

        await _repository.Add(post);

        post.Author = author;
        var postOutput = _mapper.Map<PostDto>(post);
        return Ok(postOutput);
    }

    [HttpPut("{id:guid}", Name = "Posts Update")]
    [ProducesResponseType(typeof(PostDto), 200)]
    public async Task<IActionResult> Update(Guid id, [FromBody] PostCreateDto postCreate)
    {
        var post = await _repository.GetPost(id);

        if (post is null) return BadRequest("Unable to find a post with the given Id");

        var author = await _jwt.GetAuthorFromRequest();
        if (author is null) return BadRequest("Unable to find the author");
        if (author.Id != post.AuthorId && !author.Admin) return Forbid();

        _mapper.Map(postCreate, post);
        post.UpdatedAt = DateTime.UtcNow;
        await _repository.Save();

        return Ok(_mapper.Map<PostDto>(post));
    }

    [HttpDelete("{id:guid}", Name = "Posts Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var post = await _repository.GetPost(id);
        if (post is null) return BadRequest("Unable to find a post with the given Id");

        var author = await _jwt.GetAuthorFromRequest();
        if (author is null) return BadRequest("Unable to find the author");
        if (author.Id != post.AuthorId && !author.Admin) return Forbid();

        await _repository.Delete(post);

        return Ok();
    }
}
