using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
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
}

public class JwtService : IJwtService
{
    // TODO: Refresh token
    private static readonly TimeSpan TokenLife = TimeSpan.FromHours(24);
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public string? GenerateToken(JwtAuthorPayload payload)
    {
        var issuer = _config["JwtSettings:Issuer"];
        var audience = _config["JwtSettings:Audience"];
        var key = Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!);

        var tokenDescriptor = new SecurityTokenDescriptor
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

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}