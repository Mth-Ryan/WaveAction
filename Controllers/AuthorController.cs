using Microsoft.AspNetCore.Mvc;
using WaveAction.Data;
using WaveAction.Dtos.Author;

namespace WaveAction.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthorController : ControllerBase
{
    private readonly ILogger<AuthorController> _logger;
    private readonly BlogContext _blogContext;

    public AuthorController(ILogger<AuthorController> logger, BlogContext blogContext)
    {
        _logger = logger;
        _blogContext = blogContext;
    }

    [HttpGet(Name = "Author Show")]
    [ProducesResponseType(typeof(AuthorDto), 200)]
    public async Task<IActionResult> Show()
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