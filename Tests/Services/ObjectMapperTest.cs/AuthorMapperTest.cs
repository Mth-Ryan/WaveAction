using WaveActionApi.Dtos.Author;
using WaveActionApi.Models;

namespace WaveActionApiTest.Services.ObjectMapperTest.cs;

public partial class ObjectMapperTest
{
    [Fact]
    public void ShouldMapToProfileDto()
    {
        var input = new ProfileModel
        {
            FirstName = "John",
            LastName = "Doe",
            Title = "Owner",
            ShortBio = "This is Me",
            Bio = "Hey, this is real me",
            PublicEmail = "johndoe@example.com",
            AvatarUrl = "https://example.com/avatar"
        };

        var expected = new AuthorProfileDto
        {
            FirstName = input.FirstName,
            LastName = input.LastName,
            Title = input.Title,
            ShortBio = input.ShortBio,
            Bio = input.Bio,
            PublicEmail = input.PublicEmail,
            AvatarUrl = input.AvatarUrl,
        };
        
        var actual = Mapper.Map<AuthorProfileDto>(input);
        
        Assert.Equal(expected.FirstName, actual.FirstName);
        Assert.Equal(expected.LastName, actual.LastName);
        Assert.Equal(expected.Title, actual.Title);
        Assert.Equal(expected.ShortBio, actual.ShortBio);
        Assert.Equal(expected.Bio, actual.Bio);
        Assert.Equal(expected.PublicEmail, actual.PublicEmail);
        Assert.Equal(expected.AvatarUrl, actual.AvatarUrl);
    }
}