using System.ComponentModel.DataAnnotations;

namespace WaveAction.Dtos.Posts;

public class PostCreateDto
{
    public string? ThumbnailUrl { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string? Title { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public string? Content { get; set; }

    public List<string>? Tags { get; set; }
}