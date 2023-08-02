using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaveAction.Application.Dtos.Posts;
using WaveAction.Application.Dtos.Shared;
using WaveAction.Application.Dtos.Threads;
using WaveAction.Application.Interfaces;
using WaveAction.Domain.Specification;
using WaveAction.Infrastructure.Interfaces;

namespace WaveAction.Rest.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ThreadsController : ControllerBase
{
    private readonly ILogger<ThreadsController> _logger;
    private readonly IThreadsAppService _threads;
    private readonly IJwtService _jwt;

    public ThreadsController(
        ILogger<ThreadsController> logger,
        IThreadsAppService threads,
        IJwtService jwt)
    {
        _logger = logger;
        _threads = threads;
        _jwt = jwt;
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}", Name = "Threads Get")]
    [ProducesResponseType(typeof(ThreadDto), 200)]
    public async Task<IActionResult> Get(Guid id)
    {
        // TODO: Better Error Handling
        return Ok(await _threads.Get(id));
    }

    [AllowAnonymous]
    [HttpGet("{titleSlug}", Name = "Threads Get From Title Slug")]
    [ProducesResponseType(typeof(ThreadDto), 200)]
    public async Task<IActionResult> Get(string titleSlug)
    {
        // TODO: Better Error Handling
        return Ok(await _threads.Get(titleSlug));
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}/Posts", Name = "Threads Get Posts")]
    [ProducesResponseType(typeof(PaginatedDataDto<PostShortDto>), 200)]
    public async Task<IActionResult> GetPosts(Guid id, [FromQuery] QueryOptions options)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        // TODO: Better Error Handling
        return Ok(await _threads.GetPosts(id, options));
    }

    [AllowAnonymous]
    [HttpGet("{titleSlug}/Posts", Name = "Threads Get Posts From Title Slug")]
    [ProducesResponseType(typeof(List<PostShortDto>), 200)]
    public async Task<IActionResult> GetPosts(string titleSlug, [FromQuery] QueryOptions options)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        // TODO: Better Error Handling
        return Ok(await _threads.GetPosts(titleSlug, options));
    }

    [AllowAnonymous]
    [HttpGet(Name = "Threads Get All")]
    [ProducesResponseType(typeof(PaginatedDataDto<ThreadDto>), 200)]
    public async Task<IActionResult> Get([FromQuery] QueryOptions options)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(await _threads.GetAll(options));
    }

    [HttpPost(Name = "Threads Create")]
    [ProducesResponseType(typeof(ThreadDto), 200)]
    public async Task<IActionResult> Create([FromBody] ThreadCreateDto input)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        // TODO: Better error handling
        var authorId = _jwt.GetAuthorIdFromRequest();
        if (authorId is null) return Forbid();

        // TODO: Better error handling
        return Ok(await _threads.Create(authorId.Value, input));
    }

    [HttpPut("{id:guid}", Name = "Threads Update")]
    [ProducesResponseType(typeof(ThreadDto), 200)]
    public async Task<IActionResult> Update(Guid id, [FromBody] ThreadCreateDto input)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        // TODO: Better error handling
        var authorId = _jwt.GetAuthorIdFromRequest();
        if (authorId is null) return Forbid();

        // TODO: Better error handling
        return Ok(await _threads.Update(authorId.Value, id, input));
    }

    [HttpDelete("{id:guid}", Name = "Threads Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        // TODO: Better error handling
        var authorId = _jwt.GetAuthorIdFromRequest();
        if (authorId is null) return Forbid();

        // TODO: Better error handling
        await _threads.Delete(authorId.Value, id);

        return Ok();
    }
}
