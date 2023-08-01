namespace WaveAction.Application.Specification;

public class JwtAuthorPayload
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public required string FullName { get; set; }
    public required string AvatarUrl { get; set; }
    public required bool Admin { get; set; }
}
