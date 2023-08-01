using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
public class ThreadsController : ControllerBase
{
    private readonly ILogger<ThreadsController> _logger;
    private readonly IThreadsRepository _repository;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwt;

    public ThreadsController(
        ILogger<ThreadsController> logger,
        IThreadsRepository repository,
        IMapper mapper,
        IJwtService jwt)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
        _jwt = jwt;
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}", Name = "Threads Get")]
    [ProducesResponseType(typeof(ThreadDto), 200)]
    public async Task<IActionResult> Get(Guid id)
    {
        var thread = await _repository.GetThread(id);

        return thread is null
            ? BadRequest("Unable to find a thread with the given Id")
            : Ok(_mapper.Map<ThreadDto>(thread));
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}/Posts", Name = "Threads Get Posts")]
    [ProducesResponseType(typeof(PaginatedDataDto<PostShortDto>), 200)]
    public async Task<IActionResult> GetPosts(Guid id, [FromQuery] QueryOptions options)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var total = await _repository.GetThreadsCount(options);
        var posts = await _repository.GetThreadPosts(id, options);
        var data = _mapper.Map<List<PostModel>, List<PostShortDto>>(posts);

        return Ok(new PaginatedDataDto<PostShortDto>
        {
            Search = options.SimpleSearch,
            Page = options.Page,
            PageSize = options.PageSize,
            ItemsTotalCount = (uint)total,
            Order = options.OrderBy,
            Data = data!
        });
    }

    [AllowAnonymous]
    [HttpGet("{titleSlug}/Posts", Name = "Threads Get Posts From Title Slug")]
    [ProducesResponseType(typeof(List<PostShortDto>), 200)]
    public async Task<IActionResult> GetPosts(string titleSlug, [FromQuery] QueryOptions options)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var total = await _repository.GetThreadPostsCount(titleSlug, options);
        var posts = await _repository.GetThreadPosts(titleSlug, options);

        var data = _mapper.Map<List<PostModel>, List<PostShortDto>>(posts);

        return Ok(new PaginatedDataDto<PostShortDto>
        {
            Search = options.SimpleSearch,
            Page = options.Page,
            PageSize = options.PageSize,
            ItemsTotalCount = (uint)total,
            Order = options.OrderBy,
            Data = data!
        });
    }

    [AllowAnonymous]
    [HttpGet("{titleSlug}", Name = "Threads Get From Title Slug")]
    [ProducesResponseType(typeof(ThreadDto), 200)]
    public async Task<IActionResult> Get(string titleSlug)
    {
        var thread = await _repository.GetThread(titleSlug);

        return thread is null
            ? BadRequest("Unable to find a thread with the given Title")
            : Ok(_mapper.Map<ThreadDto>(thread));
    }


    [AllowAnonymous]
    [HttpGet(Name = "Threads Get All")]
    [ProducesResponseType(typeof(PaginatedDataDto<ThreadDto>), 200)]
    public async Task<IActionResult> Get([FromQuery] QueryOptions options)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var total = await _repository.GetThreadsCount(options);
        var threads = await _repository.GetThreads(options);
        var data = _mapper.Map<List<ThreadModel>, List<ThreadDto>>(threads);

        return Ok(new PaginatedDataDto<ThreadDto>
        {
            Search = options.SimpleSearch,
            Page = options.Page,
            PageSize = options.PageSize,
            ItemsTotalCount = (uint)total,
            Order = options.OrderBy,
            Data = data!
        });
    }

    [HttpPost(Name = "Threads Create")]
    [ProducesResponseType(typeof(ThreadDto), 200)]
    public async Task<IActionResult> Create([FromBody] ThreadCreateDto threadCreate)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var author = await _jwt.GetAuthorFromRequest();
        if (author is null) return BadRequest("Unable to find the author");

        var thread = _mapper.Map<ThreadModel>(threadCreate);
        thread.AuthorId = author.Id;

        await _repository.Add(thread);

        thread.Author = author;
        var threadOutput = _mapper.Map<ThreadDto>(thread);
        return Ok(threadOutput);
    }

    [HttpPut("{id:guid}", Name = "Threads Update")]
    [ProducesResponseType(typeof(ThreadDto), 200)]
    public async Task<IActionResult> Update(Guid id, [FromBody] ThreadCreateDto threadCreate)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var thread = await _repository.GetThread(id);

        if (thread is null) return BadRequest("Unable to find a thread with the given Id");

        var author = await _jwt.GetAuthorFromRequest();
        if (author is null) return BadRequest("Unable to find the author");
        if (author.Id != thread.AuthorId && !author.Admin) return Forbid();

        _mapper.Map(threadCreate, thread);
        thread.UpdatedAt = DateTime.UtcNow;
        await _repository.Save();

        return Ok(_mapper.Map<ThreadDto>(thread));
    }

    [HttpDelete("{id:guid}", Name = "Threads Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var thread = await _repository.GetThread(id);
        if (thread is null) return BadRequest("Unable to find a thread with the given Id");

        var author = await _jwt.GetAuthorFromRequest();
        if (author is null) return BadRequest("Unable to find the author");
        if (author.Id != thread.AuthorId && !author.Admin) return Forbid();

        await _repository.Delete(thread);

        return Ok();
    }
}
