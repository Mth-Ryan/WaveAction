using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaveActionApi.Data;
using WaveActionApi.Dtos.Author;

namespace WaveActionApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]/[Action]")]
public class AuthorController : ControllerBase
{
    private readonly ILogger<AuthorController> _logger;
    private readonly BlogContext _blogContext;
    private readonly IMapper _mapper;

    public AuthorController(ILogger<AuthorController> logger, BlogContext blogContext, IMapper mapper)
    {
        _logger = logger;
        _blogContext = blogContext;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}", Name = "Author Get")]
    [ProducesResponseType(typeof(AuthorDto), 200)]
    public async Task<IActionResult> Get(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(new AuthorDto());
    }

    [HttpPut(Name = "Author Profile Edit")]
    [ProducesResponseType(typeof(AuthorDto), 200)]
    public async Task<IActionResult> EditProfile([FromBody] AuthorProfileDto profile)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(new AuthorDto());
    }
}