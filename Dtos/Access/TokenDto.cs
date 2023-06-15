using System.ComponentModel.DataAnnotations;

namespace WaveAction.Dtos.Access;

public class TokenDto
{
    [Required]
    public string? Token { get; set; }
}