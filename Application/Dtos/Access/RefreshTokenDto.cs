using System.ComponentModel.DataAnnotations;

namespace WaveAction.Application.Dtos.Access;

public class RefreshTokenDto
{
    [Required] public required string Refresh { get; set; }
}
