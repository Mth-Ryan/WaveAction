using Microsoft.EntityFrameworkCore;
using WaveActionApi.Data;
using WaveActionApi.Models;

namespace WaveActionApi.Repositories;

public interface IAccessRepository
{
    Task<AuthorModel?> GetAuthorByUsernameOrEmail(string usernameOrEmail);
    Task<int> AddAuthor(AuthorModel author);
}

public class AccessRepository : IAccessRepository
{
    private readonly BlogContext _blogContext;
    
    public AccessRepository(BlogContext blogContext)
    {
        this._blogContext = blogContext;
    }
    
    public Task<AuthorModel?> GetAuthorByUsernameOrEmail(string usernameOrEmail)
    {
        return _blogContext.Authors
            .Include(a => a.Profile)
            .SingleOrDefaultAsync(a => a.UserName == usernameOrEmail || a.Email == usernameOrEmail);
    }

    public Task<int> AddAuthor(AuthorModel author)
    {
        _blogContext.Add(author);
        return _blogContext.SaveChangesAsync();
    }
}