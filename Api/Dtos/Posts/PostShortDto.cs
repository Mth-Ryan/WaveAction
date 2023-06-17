using WaveActionApi.Dtos.Author;

namespace WaveActionApi.Dtos.Posts;

public class PostShortDto
{
    public Guid Id { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public List<string>? TagsList { get; set; }
    public AuthorShortDto? Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}