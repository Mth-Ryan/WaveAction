using WaveAction.Domain.Models;
using WaveAction.Infrastructure.Specification;

namespace WaveAction.Infrastructure.Interfaces;

public interface IRefreshTokenService
{
    public Task SaveRefreshToken(string bareToken, JwtAuthorPayload payload);
    public Task RemoveRefreshToken(string token);
    public Task<JwtAuthorPayload?> GetPayloadFromRefreshToken(string token);
    public Task<string?> GenerateRefreshToken(JwtAuthorPayload payload);
    public Task<string?> GenerateRefreshToken(AuthorModel author);
    public Task<string?> RefreshJwt(string refreshToken);
}
