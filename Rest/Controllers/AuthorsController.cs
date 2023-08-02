using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaveAction.Application.Dtos.Author;
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
public class AuthorsController : ControllerBase
{
    private readonly ILogger<AuthorsController> _logger;
    private readonly IAuthorsAppService _authors;
    private readonly IJwtService _jwt;

    public AuthorsController(
        ILogger<AuthorsController> logger,
        IAuthorsAppService authors,
        IJwtService jwt)
    {
        _logger = logger;
        _authors = authors;
        _jwt = jwt;
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}", Name = "Author Get")]
    [ProducesResponseType(typeof(AuthorDto), 200)]
    public async Task<IActionResult> Get(Guid id)
    {
        // TODO: Better error handling
        return Ok(await _authors.Get(id));
    }

    [AllowAnonymous]
    [HttpGet("", Name = "Author Get All")]
    [ProducesResponseType(typeof(PaginatedDataDto<AuthorShortDto>), 200)]
    public async Task<IActionResult> Get([FromQuery] QueryOptions options)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        // TODO: Better error handling
        return Ok(await _authors.GetAll(options));
    }

    [AllowAnonymous]
    [HttpGet("{userName}/Posts", Name = "Author Get Posts From Username")]
    [ProducesResponseType(typeof(PaginatedDataDto<PostShortDto>), 200)]
    public async Task<IActionResult> Posts(string userName, [FromQuery] QueryOptions options)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        // TODO: Better error handling
        return Ok(await _authors.GetPosts(userName, options));
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}/Posts", Name = "Author Get Posts")]
    [ProducesResponseType(typeof(PaginatedDataDto<PostShortDto>), 200)]
    public async Task<IActionResult> Posts(Guid id, [FromQuery] QueryOptions options)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        // TODO: Better error handling
        return Ok(await _authors.GetPosts(id, options));
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}/Threads", Name = "Author Get All Threads")]
    [ProducesResponseType(typeof(PaginatedDataDto<ThreadDto>), 200)]
    public async Task<IActionResult> Threads(Guid id, [FromQuery] QueryOptions options)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        // TODO: Better error handling
        return Ok(await _authors.GetThreads(id, options));
    }

    [AllowAnonymous]
    [HttpGet("{userName}/Threads", Name = "Author Get All Threads From Username")]
    [ProducesResponseType(typeof(PaginatedDataDto<ThreadDto>), 200)]
    public async Task<IActionResult> Threads(string userName, [FromQuery] QueryOptions options)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        // TODO: Better error handling
        return Ok(await _authors.GetThreads(userName, options));
    }

    [HttpPut("Profile", Name = "Author Profile Edit")]
    [ProducesResponseType(typeof(AuthorDto), 200)]
    public async Task<IActionResult> EditProfile([FromBody] AuthorProfileDto input)
    {
        // TODO: Better error handling
        var authorId = _jwt.GetAuthorIdFromRequest();
        if (authorId is null) return Forbid();

        // TODO: Better error handling
        return Ok(await _authors.UpdateProfile(authorId.Value, input));
    }
}
