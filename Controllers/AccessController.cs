using Microsoft.AspNetCore.Mvc;
using WaveAction.Data;
using WaveAction.Dtos.Access;

namespace WaveAction.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AccessController : ControllerBase
{
    private readonly ILogger<AccessController> _logger;
    private readonly BlogContext _blogContext;

    public AccessController(ILogger<AccessController> logger, BlogContext blogContext)
    {
        _logger = logger;
        _blogContext = blogContext;
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