using System.ComponentModel.DataAnnotations;

namespace WaveActionApi.Dtos.Access;

public class RefreshTokenDto
{
    [Required] public required string Refresh { get; set; }
}