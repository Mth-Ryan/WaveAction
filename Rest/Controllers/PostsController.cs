using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaveAction.Application.Dtos.Posts;
using WaveAction.Application.Dtos.Shared;
using WaveAction.Application.Interfaces;
using WaveAction.Domain.Specification;
using WaveAction.Infrastructure.Interfaces;

namespace WaveAction.Rest.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly ILogger<PostsController> _logger;
    private readonly IPostsAppService _posts;
    private readonly IJwtService _jwt;

    public PostsController(
        ILogger<PostsController> logger,
        IPostsAppService posts,
        IJwtService jwt)
    {
        _logger = logger;
        _posts = posts;
        _jwt = jwt;
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}", Name = "Posts Get")]
    [ProducesResponseType(typeof(PostDto), 200)]
    public async Task<IActionResult> Get(Guid id)
    {
        // TODO: Better error handling
        return Ok(await _posts.Get(id));
    }


    [AllowAnonymous]
    [HttpGet("{titleSlug}", Name = "Posts Get From Title Slug")]
    [ProducesResponseType(typeof(PostDto), 200)]
    public async Task<IActionResult> Get(string titleSlug)
    {
        // TODO: Better error handling
        return Ok(await _posts.Get(titleSlug));
    }

    [AllowAnonymous]
    [HttpGet(Name = "Posts Get All")]
    [ProducesResponseType(typeof(PaginatedDataDto<PostShortDto>), 200)]
    public async Task<IActionResult> Get([FromQuery] QueryOptions options)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        // TODO: Better error handling
        return Ok(await _posts.GetAll(options));
    }

    [HttpPost(Name = "Posts Create")]
    [ProducesResponseType(typeof(PostDto), 200)]
    public async Task<IActionResult> Create([FromBody] PostCreateDto input)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        // TODO: Better error handling
        var authorId = _jwt.GetAuthorIdFromRequest();
        if (authorId is null) return Forbid();

        // TODO: Better error handling
        return Ok(await _posts.Create(authorId.Value, input));
    }


    [HttpPut("{id:guid}", Name = "Posts Update")]
    [ProducesResponseType(typeof(PostDto), 200)]
    public async Task<IActionResult> Update(Guid id, [FromBody] PostCreateDto input)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        // TODO: Better error handling
        var authorId = _jwt.GetAuthorIdFromRequest();
        if (authorId is null) return Forbid();

        // TODO: Better error handling
        return Ok(await _posts.Update(authorId.Value, id, input));
    }


    [HttpDelete("{id:guid}", Name = "Posts Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        // TODO: Better error handling
        var authorId = _jwt.GetAuthorIdFromRequest();
        if (authorId is null) return Forbid();

        // TODO: Better error handling
        await _posts.Delete(authorId.Value, id);
        return Ok();
    }
}
