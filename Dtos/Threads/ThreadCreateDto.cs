using System.ComponentModel.DataAnnotations;

namespace WaveAction.Dtos.Threads;

public class ThreadCreateDto
{
    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string? Title { get; set; }

    [Required]
    [MaxLength(500)]
    public string? Description { get; set; }
}