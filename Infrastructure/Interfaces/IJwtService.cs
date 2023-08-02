using WaveAction.Domain.Models;
using System.Security.Claims;
using WaveAction.Infrastructure.Specification;

namespace WaveAction.Infrastructure.Interfaces;

public interface IJwtService
{
    public string? GenerateToken(JwtAuthorPayload payload);
    public string? GenerateToken(AuthorModel author);
    public IEnumerable<Claim>? GetClaimsFromRequest();
    public JwtAuthorPayload? GetPayloadFromRequest();
    public JwtAuthorPayload CreatePayloadFromAuthor(AuthorModel author);
    public Guid? GetAuthorIdFromRequest();
    public Task<AuthorModel?> GetAuthorFromRequest();
}
