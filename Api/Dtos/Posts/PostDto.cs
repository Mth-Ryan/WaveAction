using WaveActionApi.Dtos.Author;
using WaveActionApi.Dtos.Threads;

namespace WaveActionApi.Dtos.Posts;

public class PostDto
{
    public Guid Id { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Title { get; set; }
    public string? TitleSlug { get; set; }
    public string? Description { get; set; }
    public string? Content { get; set; }
    public AuthorShortDto? Author { get; set; }
    public ThreadShortDto? Thread { get; set; }
    public List<string>? TagList { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}