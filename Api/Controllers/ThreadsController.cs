using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaveActionApi.Data;
using WaveActionApi.Dtos.Posts;
using WaveActionApi.Dtos.Threads;
using WaveActionApi.Models;
using WaveActionApi.Services;

namespace WaveActionApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ThreadsController : ControllerBase
{
    private readonly ILogger<ThreadsController> _logger;
    private readonly BlogContext _blogContext;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwt;

    public ThreadsController(ILogger<ThreadsController> logger, BlogContext blogContext, IMapper mapper,
        IJwtService jwt)
    {
        _logger = logger;
        _blogContext = blogContext;
        _mapper = mapper;
        _jwt = jwt;
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}", Name = "Threads Get")]
    [ProducesResponseType(typeof(ThreadDto), 200)]
    public async Task<IActionResult> Get(Guid id)
    {
        var thread = await _blogContext.Threads
            .Include(t => t.Author).ThenInclude(a => a!.Profile)
            .FirstOrDefaultAsync(t => t.Id == id);
        
        return thread is null
            ? BadRequest("Unable to find a thread with the given Id")
            : Ok(_mapper.Map<ThreadDto>(thread));
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}/Posts", Name = "Threads Get Posts From Title")]
    [ProducesResponseType(typeof(List<PostShortDto>), 200)]
    public async Task<IActionResult> GetPosts(Guid id, uint? page, uint? pageSize)
    {
        var query = _blogContext.Posts
            .Include(p => p.Author)
            .ThenInclude(a => a!.Profile)
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
            .Where(p => p.ThreadId == id);
        
        var size = pageSize ?? 10;
        
        var posts = page is null
            ? await query.ToListAsync()
            : await query.Skip(((int)page * (int)size) - 1).Take((int)size).ToListAsync();

        return Ok(_mapper.Map<List<PostModel>, List<PostShortDto>>(posts));
    }
    
    [AllowAnonymous]
    [HttpGet("{title}/Posts", Name = "Threads Get Posts")]
    [ProducesResponseType(typeof(List<PostShortDto>), 200)]
    public async Task<IActionResult> GetPosts(string title, uint? page, uint? pageSize)
    {
        var query = _blogContext.Posts
            .Include(p => p.Author)
            .ThenInclude(a => a!.Profile)
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
            .Where(p => p.Thread!.Title == title);
        
        var size = pageSize ?? 10;
        
        var posts = page is null
            ? await query.ToListAsync()
            : await query.Skip(((int)page * (int)size) - 1).Take((int)size).ToListAsync();

        return Ok(_mapper.Map<List<PostModel>, List<PostShortDto>>(posts));
    }

    [AllowAnonymous]
    [HttpGet("{title}", Name = "Threads Get From Title")]
    [ProducesResponseType(typeof(ThreadDto), 200)]
    public async Task<IActionResult> Get(string title)
    {
        var thread = await _blogContext.Threads
            .Include(t => t.Author).ThenInclude(a => a!.Profile)
            .FirstOrDefaultAsync(t => t.Title == title);
        
        return thread is null
            ? BadRequest("Unable to find a thread with the given Title")
            : Ok(_mapper.Map<ThreadDto>(thread));
    }


    [AllowAnonymous]
    [HttpGet(Name = "Threads Get All")]
    [ProducesResponseType(typeof(List<ThreadDto>), 200)]
    public async Task<IActionResult> Get(uint? page, uint? pageSize)
    {
        var query = _blogContext.Threads
            .Include(t => t.Author)
            .ThenInclude(a => a!.Profile)
            .OrderByDescending(t => t.CreatedAt);

        var size = pageSize ?? 10;

        var threads = page is null
            ? await query.ToListAsync()
            : await query.Skip(((int)page * (int)size) - 1).Take((int)size).ToListAsync();

        return Ok(_mapper.Map<List<ThreadModel>, List<ThreadDto>>(threads));
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

        _blogContext.Threads.Add(thread);
        await _blogContext.SaveChangesAsync();

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

        var thread = await _blogContext.Threads
            .Include(t => t.Author)
            .ThenInclude(a => a!.Profile)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (thread is null) return BadRequest("Unable to find a thread with the given Id");

        var author = await _jwt.GetAuthorFromRequest();
        if (author is null) return BadRequest("Unable to find the author");
        if (author.Id != thread.AuthorId && !author.Admin) return Forbid();

        _mapper.Map(threadCreate, thread);
        await _blogContext.SaveChangesAsync();

        return Ok(_mapper.Map<ThreadDto>(thread));
    }

    [HttpDelete("{id:guid}", Name = "Threads Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var thread = await _blogContext.Threads
            .Include(t => t.Author)
            .FirstOrDefaultAsync(t => t.Id == id);
        if (thread is null) return BadRequest("Unable to find a thread with the given Id");

        var author = await _jwt.GetAuthorFromRequest();
        if (author is null) return BadRequest("Unable to find the author");
        if (author.Id != thread.AuthorId && !author.Admin) return Forbid();

        _blogContext.Threads.Remove(thread);
        await _blogContext.SaveChangesAsync();

        return Ok();
    }
}