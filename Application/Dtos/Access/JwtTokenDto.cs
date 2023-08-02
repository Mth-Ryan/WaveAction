using System.ComponentModel.DataAnnotations;

namespace WaveAction.Application.Dtos.Access;

public class JwtTokenDto
{
    [Required] public string? Token { get; set; }
}
