using WaveAction.Domain.Models;
using System.Security.Claims;
using WaveAction.Application.Specification;

namespace WaveAction.Application.Interfaces;

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
