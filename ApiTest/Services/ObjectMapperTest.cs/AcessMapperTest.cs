using WaveActionApi.Dtos.Access;
using WaveActionApi.Models;
using BC = BCrypt.Net.BCrypt;

namespace WaveActionApiTest.Services.ObjectMapperTest.cs;

public partial class ObjectMapperTest
{
    [Fact]
    public void ShouldMapFromSignupProfileToModel()
    {
        var input = new SignupProfileDto
        {
            FirstName = "John",
            LastName = "Doe",
        };
        
        var expected = new ProfileModel
        {
            FirstName = "John",
            LastName = "Doe",
        };

        var output = Mapper.Map<ProfileModel>(input);
        
        Assert.Equal(output.FirstName, expected.FirstName);
        Assert.Equal(output.LastName, expected.LastName);
    }

    [Fact]
    public void ShouldMapFromSignupAuthorToModel()
    {
        var input = new SignupDto
        {
            UserName = "John-Doe",
            Email = "johndoe@example.com",
            Password = "SomeValidPass*212",
            Profile = new SignupProfileDto
            {
                FirstName = "John",
                LastName = "Doe"
            }
        };
        
        var expected = new AuthorModel
        {
            UserName = input.UserName,
            Email = input.Email,
            PasswordHash = BC.HashPassword(input.Password),
            Profile = new ProfileModel
            {
                FirstName = input.Profile.FirstName,
                LastName = input.Profile.LastName
            }
        };

        var output = Mapper.Map<AuthorModel>(input);
        Assert.Equal(output.UserName, expected.UserName);
        Assert.Equal(output.Email, expected.Email);
        Assert.True(BC.Verify(input.Password, expected.PasswordHash));
        Assert.Equal(output.Profile.FirstName, expected.Profile.FirstName);
        Assert.Equal(output.Profile.LastName, expected.Profile.LastName);
    }
}