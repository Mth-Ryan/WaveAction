using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaveActionApi.Dtos.Author;
using WaveActionApi.Dtos.Posts;
using WaveActionApi.Dtos.Shared;
using WaveActionApi.Dtos.Threads;
using WaveActionApi.Models;
using WaveActionApi.Repositories;
using WaveActionApi.Services;

namespace WaveActionApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly ILogger<AuthorsController> _logger;
    private readonly IAuthorsRepository _repository;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwt;

    public AuthorsController(
        ILogger<AuthorsController> logger,
        IAuthorsRepository repository,
        IMapper mapper,
        IJwtService jwt)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
        _jwt = jwt;
    }
    
    [AllowAnonymous]
    [HttpGet("{id:guid}", Name = "Author Get")]
    [ProducesResponseType(typeof(AuthorDto), 200)]
    public async Task<IActionResult> Get(Guid id)
    {
        var author = await _repository.GetAuthor(id);
        if (author is null) return BadRequest("Unable to find a author with the given Id");

        return Ok(_mapper.Map<AuthorDto>(author));
    }
    
    [AllowAnonymous]
    [HttpGet("", Name = "Author Get All")]
    [ProducesResponseType(typeof(PaginatedDataDto<AuthorShortDto>), 200)]
    public async Task<IActionResult> Get([FromQuery] QueryOptions options)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        
        var total = await _repository.GetAuthorsCount();
        var authors = await _repository.GetAuthors(options);
        var data = _mapper.Map<List<AuthorModel>, List<AuthorShortDto>>(authors);

        return Ok(new PaginatedDataDto<AuthorShortDto>
        {
            Page = options.Page,
            PageSize = options.PageSize,
            ItemsTotalCount = (uint)total,
            Data = data!
        });
    }
    
    [AllowAnonymous]
    [HttpGet("{userName}/Posts", Name = "Author Get Posts From Username")]
    [ProducesResponseType(typeof(PaginatedDataDto<PostShortDto>), 200)]
    public async Task<IActionResult> Posts(string userName, [FromQuery] QueryOptions options)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        
        var total = await _repository.GetAuthorPostsCount(userName);
        var posts = await _repository.GetAuthorPosts(userName, options);
        var data = _mapper.Map<List<PostModel>, List<PostShortDto>>(posts);

        return Ok(new PaginatedDataDto<PostShortDto>
        {
            Page = options.Page,
            PageSize = options.PageSize,
            ItemsTotalCount = (uint)total,
            Data = data!
        });
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}/Posts", Name = "Author Get Posts")]
    [ProducesResponseType(typeof(PaginatedDataDto<PostShortDto>), 200)]
    public async Task<IActionResult> Posts(Guid id, [FromQuery] QueryOptions options)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        
        var total = await _repository.GetAuthorPostsCount(id);
        var posts = await _repository.GetAuthorPosts(id, options);
        var data = _mapper.Map<List<PostModel>, List<PostShortDto>>(posts);

        return Ok(new PaginatedDataDto<PostShortDto>
        {
            Page = options.Page,
            PageSize = options.PageSize,
            ItemsTotalCount = (uint)total,
            Data = data!
        });
    }
    
    [AllowAnonymous]
    [HttpGet("{id:guid}/Threads", Name = "Author Get All Threads")]
    [ProducesResponseType(typeof(PaginatedDataDto<ThreadDto>), 200)]
    public async Task<IActionResult> Threads(Guid id, [FromQuery] QueryOptions options)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        
        var total = await _repository.GetAuthorThreadsCount(id);
        var threads = await _repository.GetAuthorThreads(id, options);
        var data = _mapper.Map<List<ThreadModel>, List<ThreadDto>>(threads);

        return Ok(new PaginatedDataDto<ThreadDto>
        {
            Page = options.Page,
            PageSize = options.PageSize,
            ItemsTotalCount = (uint)total,
            Data = data!
        });
    }
    
    [AllowAnonymous]
    [HttpGet("{userName}/Threads", Name = "Author Get All Threads From Username")]
    [ProducesResponseType(typeof(PaginatedDataDto<ThreadDto>), 200)]
    public async Task<IActionResult> Threads(string userName, [FromQuery] QueryOptions options)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        
        var total = await _repository.GetAuthorThreadsCount(userName);
        var threads = await _repository.GetAuthorThreads(userName, options);
        var data = _mapper.Map<List<ThreadModel>, List<ThreadDto>>(threads);

        return Ok(new PaginatedDataDto<ThreadDto>
        {
            Page = options.Page,
            PageSize = options.PageSize,
            ItemsTotalCount = (uint)total,
            Data = data!
        });
    }

    [HttpPut("Profile", Name = "Author Profile Edit")]
    [ProducesResponseType(typeof(AuthorDto), 200)]
    public async Task<IActionResult> EditProfile([FromBody] AuthorProfileDto profile)
    {
        var author = await _jwt.GetAuthorFromRequest();
        if (author is null) return BadRequest("Unable to find the user");

        _mapper.Map(profile, author.Profile);
        await _repository.Save();

        return Ok(_mapper.Map<AuthorDto>(author));
    }
}