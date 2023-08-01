using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WaveActionApi.Models;

[Index(nameof(UserName), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class AuthorModel
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    [Required] public string UserName { get; set; } = "";

    [Required] public string Email { get; set; } = "";

    [Required] public string PasswordHash { get; set; } = "";

    [Required] public bool Admin { get; set; } = false;

    public bool Confirmation { get; set; }

    public bool Active { get; set; }

    public ProfileModel Profile { get; set; } = new ProfileModel();

    public ICollection<ThreadModel> Threads { get; set; } = new List<ThreadModel>();

    public ICollection<PostModel> Posts { get; set; } = new List<PostModel>();
}