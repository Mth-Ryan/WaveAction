using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using WaveAction.Domain.Interfaces;
using WaveAction.Domain.Models;
using WaveAction.Infrastructure.Interfaces;
using WaveAction.Infrastructure.Specification;

namespace WaveAction.Infrastructure.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private static readonly TimeSpan RefreshTokenLife = TimeSpan.FromDays(31);

    private readonly IDistributedCache _cacheContext;
    private readonly IJwtService _jwt;
    private readonly IAuthorsRepository _repository;

    public RefreshTokenService(
        IDistributedCache cache,
        IAuthorsRepository repository,
        IJwtService jwt)
    {
        _cacheContext = cache;
        _repository = repository;
        _jwt = jwt;
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
        return await GenerateRefreshToken(_jwt.CreatePayloadFromAuthor(author));
    }

    public async Task<JwtAuthorPayload?> GetPayloadFromRefreshToken(string token)
    {
        var bareValue = await _cacheContext.GetAsync(token);
        if (bareValue is null) return null;

        var payload = JsonSerializer.Deserialize<JwtAuthorPayload>(Encoding.UTF8.GetString(bareValue));
        return payload;
    }

    public async Task<string?> RefreshJwt(string refreshToken)
    {
        var payload = await GetPayloadFromRefreshToken(refreshToken);
        if (payload is null) return null;

        var author = await _repository.GetAuthor(payload.Id);
        if (author is null) return null;

        var jwt = _jwt.GenerateToken(author);
        await _cacheContext.RefreshAsync(refreshToken);

        return jwt;
    }

    public async Task RemoveRefreshToken(string token)
    {
        await _cacheContext.RemoveAsync(token);
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
}
