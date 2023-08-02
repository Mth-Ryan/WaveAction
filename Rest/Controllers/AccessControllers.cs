using Microsoft.AspNetCore.Mvc;
using WaveAction.Application.Dtos.Access;
using WaveAction.Application.Interfaces;
using WaveAction.Infrastructure.Interfaces;

namespace WaveAction.Rest.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AccessController : ControllerBase
{
    private readonly ILogger<AccessController> _logger;
    private readonly IAccessAppService _access;
    private readonly IJwtService _jwt;
    private readonly IRefreshTokenService _refresh;

    public AccessController(
        ILogger<AccessController> logger,
        IAccessAppService access,
        IJwtService jwt,
        IRefreshTokenService refresh)
    {
        _logger = logger;
        _access = access;
        _jwt = jwt;
        _refresh = refresh;
    }

    [HttpPost(Name = "Login")]
    [ProducesResponseType(typeof(TokensDto), 200)]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        // TODO: Better error handling
        var author = await _access.Login(login);

        var jwt = _jwt.GenerateToken(author);
        var refresh = await _refresh.GenerateRefreshToken(author);

        return (jwt is null || refresh is null)
            ? StatusCode(500, "Unable to create te JWT or Refresh Token")
            : Ok(new TokensDto { Jwt = jwt, Refresh = refresh });
    }

    [HttpPost(Name = "Signup")]
    [ProducesResponseType(typeof(TokensDto), 200)]
    public async Task<IActionResult> Signup([FromBody] SignupDto signup)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var author = await _access.Signup(signup);

        var jwt = _jwt.GenerateToken(author);
        var refresh = await _refresh.GenerateRefreshToken(author);

        return (jwt is null || refresh is null)
            ? StatusCode(500, "Unable to create te JWT or Refresh Token")
            : Ok(new TokensDto { Jwt = jwt, Refresh = refresh });
    }

    [HttpPost(Name = "Refresh")]
    [ProducesResponseType(typeof(TokensDto), 200)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto refresh)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var jwt = await _refresh.RefreshJwt(refresh.Refresh);

        return (jwt is null)
            ? BadRequest("Invalid refresh token")
            : Ok(new TokensDto { Jwt = jwt, Refresh = refresh.Refresh });
    }

    [HttpPost(Name = "Logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenDto input)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        await _refresh.RemoveRefreshToken(input.Refresh);
        return Ok();
    }
}
