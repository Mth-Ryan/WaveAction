using System.ComponentModel.DataAnnotations;

namespace WaveAction.Dtos.Access;

public class LoginDto
{
    [Required]
    public string? UserNameOrEmail { get; set; }

    [Required]
    public string? Password { get; set; }
}