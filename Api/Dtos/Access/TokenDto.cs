using System.ComponentModel.DataAnnotations;

namespace WaveActionApi.Dtos.Access;

public class TokenDto
{
    [Required]
    public string? Token { get; set; }
}