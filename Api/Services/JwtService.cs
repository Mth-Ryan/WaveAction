using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using WaveActionApi.Models;
using WaveActionApi.Repositories;
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
    public Task SaveRefreshToken(string bareToken, JwtAuthorPayload payload);
    public Task RemoveRefreshToken(string token);
    public Task<JwtAuthorPayload?> GetPayloadFromRefreshToken(string token);
    public Task<string?> GenerateRefreshToken(JwtAuthorPayload payload);
    public Task<string?> GenerateRefreshToken(AuthorModel author);
    public Task<string?> RefreshJwt(string oldRefreshToken);
}

public class JwtService : IJwtService
{
    private static readonly TimeSpan JwtLife = TimeSpan.FromMinutes(15);
    private static readonly TimeSpan RefreshTokenLife = TimeSpan.FromDays(31);

    private readonly IConfiguration _config;
    private readonly HttpContext? _context;
    private readonly IAuthorsRepository _repository;
    private readonly IDistributedCache _cacheContext;

    public JwtService(
        IConfiguration config,
        IHttpContextAccessor contextAccessor,
        IAuthorsRepository repository,
        IDistributedCache cache)
    {
        _config = config;
        _context = contextAccessor.HttpContext;
        _repository = repository;
        _cacheContext = cache;
    }

    private static JwtAuthorPayload CreatePayloadFromAuthor(AuthorModel author)
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

    public async Task<AuthorModel?> GetAuthorFromRequest()
    {
        var payload = GetPayloadFromRequest();
        if (payload is null) return null;
        return await _repository.GetAuthor(payload.Id);
    }

    public async Task SaveRefreshToken(string bareToken, JwtAuthorPayload payload)
    {
        var value = JsonSerializer.Serialize(payload);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = RefreshTokenLife,
        };
        await _cacheContext.SetAsync(bareToken, Encoding.UTF8.GetBytes(value), options);
    }

    public async Task RemoveRefreshToken(string token)
    {
        await _cacheContext.RemoveAsync(token);
    }

    public async Task<JwtAuthorPayload?> GetPayloadFromRefreshToken(string token)
    {
        var bareValue = await _cacheContext.GetAsync(token);
        if (bareValue is null) return null;

        var payload = JsonSerializer.Deserialize<JwtAuthorPayload>(Encoding.UTF8.GetString(bareValue));
        return payload;
    }

    public async Task<string?> GenerateRefreshToken(JwtAuthorPayload payload)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var bareToken = Convert.ToBase64String(randomNumber);
        await SaveRefreshToken(bareToken, payload);
        return bareToken;
    }

    public async Task<string?> GenerateRefreshToken(AuthorModel author)
    {
        return await GenerateRefreshToken(CreatePayloadFromAuthor(author));
    }

    public async Task<string?> RefreshJwt(string refreshToken)
    {
        var payload = await GetPayloadFromRefreshToken(refreshToken);
        if (payload is null) return null;

        var jwt = GenerateToken(payload);
        await _cacheContext.RefreshAsync(refreshToken);

        return jwt;
    }
}