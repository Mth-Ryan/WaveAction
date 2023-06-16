using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WaveActionApi.Models;

[Index(nameof(Title), IsUnique = true)]
public class PostModel
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    [Required] public string? Title { get; set; }

    public string Description { get; set; } = "";

    [Required] public string Tags { get; set; } = "";

    [Required] public string ThumbnailUrl { get; set; } = "";

    public string Content { get; set; } = "";

    public Guid AuthorId { get; set; }

    public AuthorModel? Author { get; set; }

    public Guid ThreadId { get; set; }

    public ThreadModel? Thread { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}