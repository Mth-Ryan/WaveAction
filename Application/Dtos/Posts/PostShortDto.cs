using WaveAction.Application.Dtos.Author;
using WaveAction.Application.Dtos.Threads;

namespace WaveAction.Application.Dtos.Posts;

public class PostShortDto
{
    public Guid Id { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Title { get; set; }
    public string? TitleSlug { get; set; }
    public string? Description { get; set; }
    public List<string>? TagsList { get; set; }
    public AuthorShortDto? Author { get; set; }
    public ThreadShortDto? Thread { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
