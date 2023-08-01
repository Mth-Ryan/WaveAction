using WaveAction.Domain.Models;

namespace WaveAction.Domain.Interfaces;

public interface IAccessRepository
{
    Task<AuthorModel?> GetAuthorByUsernameOrEmail(string usernameOrEmail);
    Task<int> AddAuthor(AuthorModel author);
}
