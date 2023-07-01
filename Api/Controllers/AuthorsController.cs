using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaveActionApi.Data;
using WaveActionApi.Dtos.Author;
using WaveActionApi.Dtos.Posts;
using WaveActionApi.Dtos.Shared;
using WaveActionApi.Dtos.Threads;
using WaveActionApi.Models;
using WaveActionApi.Services;

namespace WaveActionApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly ILogger<AuthorsController> _logger;
    private readonly BlogContext _blogContext;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwt;

    public AuthorsController(ILogger<AuthorsController> logger, BlogContext blogContext, IMapper mapper,
        IJwtService jwt)
    {
        _logger = logger;
        _blogContext = blogContext;
        _mapper = mapper;
        _jwt = jwt;
    }
    
    [AllowAnonymous]
    [HttpGet("{id:guid}", Name = "Author Get")]
    [ProducesResponseType(typeof(AuthorDto), 200)]
    public async Task<IActionResult> Get(Guid id)
    {
        var author = await _blogContext.Authors
            .AsNoTracking()
            .Include(a => a.Profile)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (author is null) return BadRequest("Unable to find a author with the given Id");

        return Ok(_mapper.Map<AuthorDto>(author));
    }
    
    [AllowAnonymous]
    [HttpGet("", Name = "Author Get All")]
    [ProducesResponseType(typeof(PaginatedDataDto<AuthorShortDto>), 200)]
    public async Task<IActionResult> Get(uint page = 0, uint pageSize = 25)
    {
        if (pageSize > 1000) return BadRequest("The page size exceeds the limit of 1000");

        var total = await _blogContext.Posts.CountAsync();

        var authors = await _blogContext.Authors
            .AsNoTracking()
            .Include(a => a.Profile)
            .Select(a => new AuthorModel
            {
                Id = a.Id,
                UserName = a.UserName,
                Email = a.Email,
                Profile = new ProfileModel
                {
                    FirstName = a.Profile.FirstName,
                    LastName = a.Profile.LastName,
                    Title = a.Profile.Title,
                    ShortBio = a.Profile.ShortBio,
                    PublicEmail = a.Profile.PublicEmail,
                    AvatarUrl = a.Profile.AvatarUrl
                }
            })
            .Skip((int)page * (int)pageSize)
            .Take((int)pageSize)
            .ToListAsync();

        var data = _mapper.Map<List<AuthorModel>, List<AuthorShortDto>>(authors);

        return Ok(new PaginatedDataDto<AuthorShortDto>
        {
            Page = page,
            PageSize = pageSize,
            ItemsTotalCount = (uint)total,
            Data = data!
        });
    }
    
    [AllowAnonymous]
    [HttpGet("{userName}/Posts", Name = "Author Get Posts From Username")]
    [ProducesResponseType(typeof(PaginatedDataDto<PostShortDto>), 200)]
    public async Task<IActionResult> Posts(string userName, uint page = 0, uint pageSize = 25)
    {
        if (pageSize > 1000) return BadRequest("The page size exceeds the limit of 1000");

        var total = await _blogContext.Posts
            .Include(p => p.Author)
            .Where(p => p.Author!.UserName == userName).CountAsync();

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
            .Where(p => p.Author!.UserName == userName)
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
    [HttpGet("{id:guid}/Posts", Name = "Author Get Posts")]
    [ProducesResponseType(typeof(PaginatedDataDto<PostShortDto>), 200)]
    public async Task<IActionResult> Posts(Guid id, uint page = 0, uint pageSize = 25)
    {
        if (pageSize > 1000) return BadRequest("The page size exceeds the limit of 1000");

        var total = await _blogContext.Posts.Where(p => p.AuthorId == id).CountAsync();

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
            .Where(p => p.AuthorId == id)
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
    [HttpGet("{id:guid}/Threads", Name = "Author Get All Threads")]
    [ProducesResponseType(typeof(PaginatedDataDto<ThreadDto>), 200)]
    public async Task<IActionResult> Threads(Guid id, uint page = 0, uint pageSize = 25)
    {
        if (pageSize > 1000) return BadRequest("The page size exceeds the limit of 1000");

        var total = await _blogContext.Threads.Where(t => t.AuthorId == id).CountAsync();

        var threads = await _blogContext.Threads
            .AsNoTracking()
            .Include(t => t.Author)
            .ThenInclude(a => a!.Profile)
            .OrderByDescending(t => t.CreatedAt)
            .Where(t => t.AuthorId == id)
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
    
    [AllowAnonymous]
    [HttpGet("{userName}/Threads", Name = "Author Get All Threads From Username")]
    [ProducesResponseType(typeof(PaginatedDataDto<ThreadDto>), 200)]
    public async Task<IActionResult> Threads(string userName, uint page = 0, uint pageSize = 25)
    {
        if (pageSize > 1000) return BadRequest("The page size exceeds the limit of 1000");

        var total = await _blogContext.Threads
            .Include(t => t.Author)
            .Where(t => t.Author!.UserName == userName).CountAsync();

        var threads = await _blogContext.Threads
            .AsNoTracking()
            .Include(t => t.Author)
            .ThenInclude(a => a!.Profile)
            .OrderByDescending(t => t.CreatedAt)
            .Where(t => t.Author!.UserName == userName)
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

    [HttpPut("Profile", Name = "Author Profile Edit")]
    [ProducesResponseType(typeof(AuthorDto), 200)]
    public async Task<IActionResult> EditProfile([FromBody] AuthorProfileDto profile)
    {
        var author = await _jwt.GetAuthorFromRequest();
        if (author is null) return BadRequest("Unable to find the user");

        _mapper.Map(profile, author.Profile);
        await _blogContext.SaveChangesAsync();

        return Ok(_mapper.Map<AuthorDto>(author));
    }
}