using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WaveActionApi.Dtos.Access;
using WaveActionApi.Models;
using WaveActionApi.Repositories;
using WaveActionApi.Services;
using BC = BCrypt.Net.BCrypt;

namespace WaveActionApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AccessController : ControllerBase
{
    private readonly ILogger<AccessController> _logger;
    private readonly IAccessRepository _repository;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwt;

    public AccessController(
        ILogger<AccessController> logger,
        IAccessRepository repository,
        IMapper mapper,
        IJwtService jwt)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
        _jwt = jwt;
    }

    [HttpPost(Name = "Login")]
    [ProducesResponseType(typeof(TokensDto), 200)]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var author = await _repository.GetAuthorByUsernameOrEmail(login.UserNameOrEmail!);

        if (author is null) return BadRequest("Invalid Username or Email");

        if (!BC.Verify(login.Password, author.PasswordHash))
            return BadRequest("Invalid Password");

        var jwt = _jwt.GenerateToken(author);
        var refresh = await _jwt.GenerateRefreshToken(author);
        
        return (jwt is null  || refresh is null)
            ? StatusCode(500, "Unable to create te JWT or Refresh Token") 
            : Ok(new TokensDto { Jwt = jwt, Refresh = refresh});
    }

    [HttpPost(Name = "Signup")]
    [ProducesResponseType(typeof(TokensDto), 200)]
    public async Task<IActionResult> Signup([FromBody] SignupDto signup)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var author = _mapper.Map<AuthorModel>(signup);
        author.Profile.PublicEmail = author.Email;

        await _repository.AddAuthor(author);

        var jwt = _jwt.GenerateToken(author);
        var refresh = await _jwt.GenerateRefreshToken(author);
        
        return (jwt is null  || refresh is null)
            ? StatusCode(500, "Unable to create te JWT or Refresh Token") 
            : Ok(new TokensDto { Jwt = jwt, Refresh = refresh});
    }

    [HttpPost(Name = "Refresh")]
    [ProducesResponseType(typeof(TokensDto), 200)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto refresh)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        
        var jwt = await _jwt.RefreshJwt(refresh.Refresh);
        
        return (jwt is null)
            ? StatusCode(500, "Unable to create te JWT") 
            : Ok(new TokensDto { Jwt = jwt, Refresh = refresh.Refresh});
    }
}