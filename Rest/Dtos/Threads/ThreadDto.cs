using WaveActionApi.Dtos.Author;

namespace WaveActionApi.Dtos.Threads;

public class ThreadDto
{
    public Guid Id { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Title { get; set; }
    public string? TitleSlug { get; set; }
    public string? Description { get; set; }
    public AuthorShortDto? Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}