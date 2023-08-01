using System.ComponentModel.DataAnnotations;
using WaveActionApi.Dtos.Access;

namespace WaveActionApiTest.Dtos.Access;

public class LoginDtoTest
{
    [Fact]
    public void DtoShouldBeValid()
    {
        var login = new LoginDto
        {
            UserNameOrEmail = "John-Doe",
            Password = "SomePassword*12",
        };

        var validationContext = new ValidationContext(login, null, null);
        var result = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(login, validationContext, result, true);
        Assert.True(isValid);
    }
    
    [Fact]
    public void DtoShouldBeInvalid()
    {
        var login = new LoginDto
        {
            UserNameOrEmail = "",
            Password = "",
        };

        var validationContext = new ValidationContext(login, null, null);
        var result = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(login, validationContext, result, true);
        Assert.False(isValid);
    }
}