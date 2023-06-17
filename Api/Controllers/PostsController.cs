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
        var post = await _blogContext.Posts
            .Include(p => p.Author).ThenInclude(a => a!.Profile)
            .Include(p => p.Thread)
            .FirstOrDefaultAsync(t => t.Id == id);

        return post is null
            ? BadRequest("Unable to find a post with the given Id")
            : Ok(_mapper.Map<PostDto>(post));
    }

    [AllowAnonymous]
    [HttpGet("{title}", Name = "Posts Get From Name")]
    [ProducesResponseType(typeof(PostDto), 200)]
    public async Task<IActionResult> Get(string title)
    {
        var post = await _blogContext.Posts
            .Include(p => p.Author).ThenInclude(a => a!.Profile)
            .Include(p => p.Thread)
            .FirstOrDefaultAsync(t => t.Title == title);

        return post is null
            ? BadRequest("Unable to find a post with the given Title")
            : Ok(_mapper.Map<PostDto>(post));
    }


    [AllowAnonymous]
    [HttpGet(Name = "Posts Get All")]
    [ProducesResponseType(typeof(List<PostShortDto>), 200)]
    public async Task<IActionResult> Get(uint? pageSize, uint? page)
    {
        var query = _blogContext.Posts
            .Include(t => t.Author).ThenInclude(a => a!.Profile)
            .Include(p => p.Thread)
            .Select(p => new PostModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Tags = p.Tags,
                AuthorId = p.AuthorId,
                Author = p.Author,
                ThreadId = p.ThreadId,
                Thread = p.Thread,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
            })
            .OrderByDescending(t => t.CreatedAt);

        var size = pageSize ?? 10;

        var posts = page is null
            ? await query.ToListAsync()
            : await query.Skip(((int)page * (int)size) - 1).Take((int)size).ToListAsync();

        return Ok(_mapper.Map<List<PostModel>, List<PostShortDto>>(posts));
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
    public async Task<IActionResult> Update(Guid id, [FromBody] PostCreateDto postCreate)
    {
        var post = await _blogContext.Posts
            .Include(p => p.Author).ThenInclude(a => a!.Profile)
            .Include(p => p.Thread)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (post is null) return BadRequest("Unable to find a post with the given Id");

        var author = await _jwt.GetAuthorFromRequest();
        if (author is null) return BadRequest("Unable to find the author");
        if (author.Id != post.AuthorId && !author.Admin) return Forbid();

        _mapper.Map(postCreate, post);
        await _blogContext.SaveChangesAsync();

        return Ok(_mapper.Map<PostDto>(post));
    }

    [HttpDelete("{id:guid}", Name = "Posts Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var post = await _blogContext.Posts
            .Include(p => p.Author)
            .FirstOrDefaultAsync(t => t.Id == id);
        if (post is null) return BadRequest("Unable to find a post with the given Id");

        var author = await _jwt.GetAuthorFromRequest();
        if (author is null) return BadRequest("Unable to find the author");
        if (author.Id != post.AuthorId && !author.Admin) return Forbid();

        _blogContext.Posts.Remove(post);
        await _blogContext.SaveChangesAsync();

        return Ok();
    }
}