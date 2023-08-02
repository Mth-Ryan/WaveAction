namespace WaveAction.Application.Dtos.Author;

public class AuthorDto
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public AuthorProfileDto? Profile { get; set; }
}
