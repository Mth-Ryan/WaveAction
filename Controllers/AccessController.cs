using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WaveActionApi.Data;
using WaveActionApi.Dtos.Access;

namespace WaveActionApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AccessController : ControllerBase
{
    private readonly ILogger<AccessController> _logger;
    private readonly BlogContext _blogContext;
    private readonly IMapper _mapper;

    public AccessController(ILogger<AccessController> logger, BlogContext blogContext, IMapper mapper)
    {
        _logger = logger;
        _blogContext = blogContext;
        _mapper = mapper;
    }

    [HttpPost(Name = "Login")]
    [ProducesResponseType(typeof(TokenDto), 200)]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(new TokenDto(){Token = "abc"});
    }

    [HttpPost(Name = "Signup")]
    [ProducesResponseType(typeof(TokenDto), 200)]
    public async Task<IActionResult> Signup([FromBody] SignupDto signup)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(new TokenDto(){Token = "abc"});
    }
}