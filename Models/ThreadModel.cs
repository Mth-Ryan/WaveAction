using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WaveAction.Models;

[Index(nameof(Title), IsUnique = true)]
public class ThreadModel
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string? Title { get; set; }

    public string? Description { get; set; }

    public Guid AuthorId { get; set; }

    public AuthorModel? Author { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}