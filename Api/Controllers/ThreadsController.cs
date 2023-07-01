using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaveActionApi.Data;
using WaveActionApi.Dtos.Posts;
using WaveActionApi.Dtos.Shared;
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
            .AsNoTracking()
            .Include(t => t.Author).ThenInclude(a => a!.Profile)
            .FirstOrDefaultAsync(t => t.Id == id);

        return thread is null
            ? BadRequest("Unable to find a thread with the given Id")
            : Ok(_mapper.Map<ThreadDto>(thread));
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}/Posts", Name = "Threads Get Posts")]
    [ProducesResponseType(typeof(PaginatedDataDto<PostShortDto>), 200)]
    public async Task<IActionResult> GetPosts(Guid id, uint page = 0, uint pageSize = 25)
    {
        if (pageSize > 1000) return BadRequest("The page size exceeds the limit of 1000");

        var total = await _blogContext.Posts.Where(p => p.ThreadId == id).CountAsync();

        var posts = await _blogContext.Posts
            .AsNoTracking()
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
            .Where(p => p.ThreadId == id)
            .Skip((int)page * (int)pageSize)
            .Take((int)pageSize)
            .ToListAsync();

        var data = _mapper.Map<List<PostModel>, List<PostShortDto>>(posts);

        return Ok(new PaginatedDataDto<PostShortDto>
        {
            Page = page,
            PageSize = pageSize,
            ItemsTotalCount = (uint)total,
            Data = data!
        });
    }

    [AllowAnonymous]
    [HttpGet("{titleSlug}/Posts", Name = "Threads Get Posts From Title Slug")]
    [ProducesResponseType(typeof(List<PostShortDto>), 200)]
    public async Task<IActionResult> GetPosts(string titleSlug, uint page = 0, uint pageSize = 25)
    {
        if (pageSize > 1000) return BadRequest("The page size exceeds the limit of 1000");

        var total = await _blogContext.Posts
            .Include(p => p.Thread)
            .Where(p => p.Thread!.TitleSlug == titleSlug)
            .CountAsync();

        var posts = await _blogContext.Posts
            .AsNoTracking()
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
            .Where(p => p.Thread!.TitleSlug == titleSlug)
            .Skip((int)page * (int)pageSize)
            .Take((int)pageSize)
            .ToListAsync();

        var data = _mapper.Map<List<PostModel>, List<PostShortDto>>(posts);

        return Ok(new PaginatedDataDto<PostShortDto>
        {
            Page = page,
            PageSize = pageSize,
            ItemsTotalCount = (uint)total,
            Data = data!
        });
    }

    [AllowAnonymous]
    [HttpGet("{titleSlug}", Name = "Threads Get From Title Slug")]
    [ProducesResponseType(typeof(ThreadDto), 200)]
    public async Task<IActionResult> Get(string titleSlug)
    {
        var thread = await _blogContext.Threads
            .AsNoTracking()
            .Include(t => t.Author).ThenInclude(a => a!.Profile)
            .FirstOrDefaultAsync(t => t.TitleSlug == titleSlug);

        return thread is null
            ? BadRequest("Unable to find a thread with the given Title")
            : Ok(_mapper.Map<ThreadDto>(thread));
    }


    [AllowAnonymous]
    [HttpGet(Name = "Threads Get All")]
    [ProducesResponseType(typeof(PaginatedDataDto<ThreadDto>), 200)]
    public async Task<IActionResult> Get(uint page = 0, uint pageSize = 25)
    {
        if (pageSize > 1000) return BadRequest("The page size exceeds the limit of 1000");

        var total = await _blogContext.Threads.CountAsync();

        var threads = await _blogContext.Threads
            .AsNoTracking()
            .Include(t => t.Author)
            .ThenInclude(a => a!.Profile)
            .OrderByDescending(t => t.CreatedAt)
            .Skip((int)page * (int)pageSize)
            .Take((int)pageSize)
            .ToListAsync();

        var data = _mapper.Map<List<ThreadModel>, List<ThreadDto>>(threads);

        return Ok(new PaginatedDataDto<ThreadDto>
        {
            Page = page,
            PageSize = pageSize,
            ItemsTotalCount = (uint)total,
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
        thread.UpdatedAt = DateTime.UtcNow;
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