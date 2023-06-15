using System.ComponentModel.DataAnnotations;

namespace WaveAction.Dtos.Threads;

public class ThreadCreateDto
{
    public string? ThumbnailUrl { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string? Title { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
}