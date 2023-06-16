using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaveActionApi.Data;
using WaveActionApi.Dtos.Access;
using WaveActionApi.Models;
using WaveActionApi.Services;
using BC = BCrypt.Net.BCrypt;

namespace WaveActionApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AccessController : ControllerBase
{
    private readonly ILogger<AccessController> _logger;
    private readonly BlogContext _blogContext;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwt;

    private static JwtAuthorPayload PayloadFormModel(AuthorModel author)
    {
        return new JwtAuthorPayload
        {
            Id = author.Id,
            Admin = author.Admin,
            UserName = author.UserName,
            FullName = $"{author.Profile.FirstName} {author.Profile.LastName}",
            Email = author.Email,
            AvatarUrl = author.Profile.AvatarUrl,
        };
    }
    
    public AccessController(ILogger<AccessController> logger, BlogContext blogContext, IMapper mapper, IJwtService jwt)
    {
        _logger = logger;
        _blogContext = blogContext;
        _mapper = mapper;
        _jwt = jwt;
    }

    [HttpPost(Name = "Login")]
    [ProducesResponseType(typeof(TokenDto), 200)]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var author = await _blogContext.Authors
            .Include(a => a.Profile)
            .FirstOrDefaultAsync(a => a.UserName == login.UserNameOrEmail || a.Email == login.UserNameOrEmail);

        if (author is null) return BadRequest("Invalid Username or Email");

        if (!BC.Verify(login.Password, author.PasswordHash))
            return BadRequest("Invalid Password");

        var payload = PayloadFormModel(author);

        var token = _jwt.GenerateToken(payload);
        
        return token is null 
            ? StatusCode(500, "Unable to create te JWT") 
            : Ok(new TokenDto() { Token = token });
    }

    [HttpPost(Name = "Signup")]
    [ProducesResponseType(typeof(TokenDto), 200)]
    public async Task<IActionResult> Signup([FromBody] SignupDto signup)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var author = _mapper.Map<AuthorModel>(signup);
        author.Profile.PublicEmail = author.Email;
        
        _blogContext.Authors.Add(author);
        await _blogContext.SaveChangesAsync();

        var payload = PayloadFormModel(author);

        var token = _jwt.GenerateToken(payload);
        
        return token is null 
            ? StatusCode(500, "Unable to create te JWT") 
            : Ok(new TokenDto() { Token = token });
    }
}