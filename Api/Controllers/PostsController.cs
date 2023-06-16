using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaveActionApi.Data;
using WaveActionApi.Dtos.Posts;
using WaveActionApi.Models;
using WaveActionApi.Services;

namespace WaveActionApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly ILogger<PostsController> _logger;
    private readonly BlogContext _blogContext;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwt;

    public PostsController(ILogger<PostsController> logger, BlogContext blogContext, IMapper mapper, IJwtService jwt)
    {
        _logger = logger;
        _blogContext = blogContext;
        _mapper = mapper;
        _jwt = jwt;
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}", Name = "Posts Get")]
    [ProducesResponseType(typeof(PostDto), 200)]
    public async Task<IActionResult> Get(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(new PostDto());
    }

    [AllowAnonymous]
    [HttpGet("{name}", Name = "Posts Get From Name")]
    [ProducesResponseType(typeof(PostDto), 200)]
    public async Task<IActionResult> Get(string name)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(new PostDto());
    }


    [HttpGet(Name = "Posts Get All")]
    [ProducesResponseType(typeof(List<PostShortDto>), 200)]
    public async Task<IActionResult> Get(uint PageSize, uint Page)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(new List<PostShortDto>());
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

        _blogContext.Posts.Add(post);
        await _blogContext.SaveChangesAsync();

        post.Author = author;
        var postOutput = _mapper.Map<PostDto>(post);
        return Ok(postOutput);
    }

    [HttpPut("{id:guid}", Name = "Posts Update")]
    [ProducesResponseType(typeof(PostDto), 200)]
    public async Task<IActionResult> Update(Guid id, [FromBody] PostCreateDto threadCreate)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(new PostDto());
    }

    [HttpDelete("{id:guid}", Name = "Posts Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok();
    }
}