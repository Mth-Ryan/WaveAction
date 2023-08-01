namespace WaveActionApi.Dtos.Threads;

public class ThreadShortDto
{
    public Guid Id { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Title { get; set; }
    public string? TitleSlug { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}