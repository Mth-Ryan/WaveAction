using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WaveActionApi.Models;

[Index(nameof(Title), IsUnique = true)]
[Index(nameof(TitleSlug), IsUnique = true)]
public class ThreadModel
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    [Required] public string? Title { get; set; }
    
    [Required] public string? TitleSlug { get; set; }

    public string Description { get; set; } = "";

    [Required] public string ThumbnailUrl { get; set; } = "";

    public Guid AuthorId { get; set; }

    public AuthorModel? Author { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}