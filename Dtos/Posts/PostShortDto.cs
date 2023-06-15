using WaveAction.Dtos.Author;

namespace WaveAction.Dtos.Posts;

public class PostShortDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public List<string>? Tags { get; set; }
    public AuthorDto? Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}