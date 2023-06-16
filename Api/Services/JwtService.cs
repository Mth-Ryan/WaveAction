using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WaveActionApi.Data;
using WaveActionApi.Models;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace WaveActionApi.Services;

public class JwtAuthorPayload
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public required string FullName { get; set; }
    public required string AvatarUrl { get; set; }
    public required bool Admin { get; set; }
}

public interface IJwtService
{
    public string? GenerateToken(JwtAuthorPayload payload);
    public string? GenerateToken(AuthorModel author);
    public IEnumerable<Claim>? GetClaimsFromRequest();
    public JwtAuthorPayload? GetPayloadFromRequest();
    public Task<AuthorModel?> GetAuthorFromRequest();
}

public class JwtService : IJwtService
{
    // TODO: Refresh token
    private static readonly TimeSpan TokenLife = TimeSpan.FromHours(24);
    private readonly IConfiguration _config;
    private readonly HttpContext? _context;
    private readonly BlogContext _blogContext;

    public JwtService(IConfiguration config, IHttpContextAccessor contextAccessor, BlogContext blogContext)
    {
        _config = config;
        _context = contextAccessor.HttpContext;
        _blogContext = blogContext;
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
            Expires = DateTime.UtcNow.Add(TokenLife),
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
        var payload = new JwtAuthorPayload
        {
            Id = author.Id,
            Email = author.Email,
            UserName = author.UserName,
            FullName = $"{author.Profile.FirstName} {author.Profile.LastName}",
            AvatarUrl = author.Profile.AvatarUrl,
            Admin = author.Admin
        };
        return GenerateToken(payload);
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

    public async Task<AuthorModel?> GetAuthorFromRequest()
    {
        var payload = GetPayloadFromRequest();
        if (payload is null) return null;
        return await _blogContext.Authors
            .Include(a => a.Profile)
            .Select(a => new AuthorModel
            {
                Id = a.Id,
                UserName = a.UserName,
                Email = a.Email,
                Admin = a.Admin,
                Profile = new ProfileModel
                {
                    FirstName = a.Profile.FirstName,
                    LastName = a.Profile.LastName,
                    Title = a.Profile.Title,
                    ShortBio = a.Profile.ShortBio,
                    PublicEmail = a.Profile.PublicEmail,
                    AvatarUrl = a.Profile.AvatarUrl,
                }
            })
            .FirstOrDefaultAsync(a => a.Id == payload.Id);
    }
}