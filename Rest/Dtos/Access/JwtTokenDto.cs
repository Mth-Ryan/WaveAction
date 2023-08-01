using System.ComponentModel.DataAnnotations;

namespace WaveActionApi.Dtos.Access;

public class JwtTokenDto
{
    [Required] public string? Token { get; set; }
}