namespace WaveAction.Dtos.Author;

public class AuthorShortDto
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public AuthorShortProfileDto? Profile { get; set; }
}