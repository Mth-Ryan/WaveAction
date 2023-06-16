using System.ComponentModel.DataAnnotations;

namespace WaveActionApi.Models;

public class ProfileModel
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    public Guid AuthorId { get; set; }

    public AuthorModel? Author { get; set; }

    [Required] public string? FirstName { get; set; }

    [Required] public string? LastName { get; set; }

    public string? Title { get; set; }

    public string? Bio { get; set; }

    public string? ShortBio { get; set; }

    [Required] public string? PublicEmail { get; set; }

    public string? AvatarUrl { get; set; }
}