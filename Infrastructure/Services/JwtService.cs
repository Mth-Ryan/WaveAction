using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WaveAction.Infrastructure.Interfaces;
using WaveAction.Infrastructure.Specification;
using WaveAction.Domain.Interfaces;
using WaveAction.Domain.Models;
using System.IdentityModel.Tokens.Jwt;

namespace WaveAction.Infrastructure.Services;

public class JwtService : IJwtService
{
    private static readonly TimeSpan JwtLife = TimeSpan.FromMinutes(15);

    private readonly IConfiguration _config;
    private readonly HttpContext? _context;
    private readonly IAuthorsRepository _repository;

    public JwtService(
        IConfiguration config,
        IHttpContextAccessor contextAccessor,
        IAuthorsRepository repository)
    {
        _config = config;
        _context = contextAccessor.HttpContext;
        _repository = repository;
    }

    public JwtAuthorPayload CreatePayloadFromAuthor(AuthorModel author)
    {
        return new JwtAuthorPayload
        {
            Id = author.Id,
            Email = author.Email,
            UserName = author.UserName,
            FullName = $"{author.Profile.FirstName} {author.Profile.LastName}",
            AvatarUrl = author.Profile.AvatarUrl,
            Admin = author.Admin
        };
    }

    private SecurityTokenDescriptor CreateTokenDescriptor(JwtAuthorPayload payload)
    {
        var issuer = _config["JwtSettings:Issuer"];
        var audience = _config["JwtSettings:Audience"];
        var key = Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!);

        return new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("authorId", payload.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, payload.UserName),
                new Claim(JwtRegisteredClaimNames.Email, payload.Email),
                new Claim("fullName", payload.FullName),
                new Claim("admin", payload.Admin.ToString()),
                new Claim("avatarUrl", payload.AvatarUrl),
            }),
            Expires = DateTime.UtcNow.Add(JwtLife),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };
    }

    public string? GenerateToken(JwtAuthorPayload payload)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(CreateTokenDescriptor(payload));
        return tokenHandler.WriteToken(token);
    }

    public string? GenerateToken(AuthorModel author)
    {
        return GenerateToken(CreatePayloadFromAuthor(author));
    }

    public IEnumerable<Claim>? GetClaimsFromRequest()
    {
        string? token = _context?.Request.Headers["Authorization"];
        if (token is null) return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwt = tokenHandler.ReadJwtToken(token["Bearer ".Length..]);
        return jwt.Claims;
    }

    public JwtAuthorPayload? GetPayloadFromRequest()
    {
        var claims = GetClaimsFromRequest()?.ToArray();
        if (claims is null) return null;

        return new JwtAuthorPayload
        {
            Id = new Guid(claims.First(c => c.Type == "authorId").Value),
            Email = claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value,
            UserName = claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value,
            FullName = claims.First(c => c.Type == "fullName").Value,
            AvatarUrl = claims.First(c => c.Type == "avatarUrl").Value,
            Admin = claims.First(c => c.Type == "avatarUrl").Value == "true",
        };
    }

    public Guid? GetAuthorIdFromRequest()
    {
        var payload = GetPayloadFromRequest();
        return payload?.Id;
    }

    public async Task<AuthorModel?> GetAuthorFromRequest()
    {
        var id = GetAuthorIdFromRequest();
        if (id is null) return null;
        return await _repository.GetAuthor(id.Value);
    }
}
