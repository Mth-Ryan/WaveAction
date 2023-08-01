using System.ComponentModel.DataAnnotations;

namespace WaveAction.Application.Dtos.Access;

public class SignupProfileDto
{
    [Required]
    [MinLength(2)]
    [MaxLength(60)]
    public string? FirstName { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(60)]
    public string? LastName { get; set; }
}

public class SignupDto
{
    [Required]
    [MinLength(2)]
    [MaxLength(60)]
    [RegularExpression(@"^[a-zA-Z_\-]+$")]
    public string? UserName { get; set; }

    [Required][EmailAddress] public string? Email { get; set; }

    [Required]
    [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
    public string? Password { get; set; }

    [Required] public SignupProfileDto? Profile { get; set; }
}
