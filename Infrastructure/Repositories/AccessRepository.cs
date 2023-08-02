using Microsoft.EntityFrameworkCore;
using WaveAction.Domain.Interfaces;
using WaveAction.Domain.Models;
using WaveAction.Infrastructure.Contexts;

namespace WaveAction.Infrastructure.Repositories;

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
