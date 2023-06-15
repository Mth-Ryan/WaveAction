using WaveAction.Dtos.Author;

namespace WaveAction.Dtos.Threads;

public class ThreadDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public AuthorDto? Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}