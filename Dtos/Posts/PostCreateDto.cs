namespace WaveAction.Dtos.Posts;

public class PostCreateDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Content { get; set; }
    public List<string>? Tags { get; set; }
}