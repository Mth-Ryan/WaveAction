using Microsoft.EntityFrameworkCore;
using WaveActionApi.Data;
using WaveActionApi.Models;

namespace WaveActionApi.Repositories;

public interface IAuthorsRepository
{
    Task<AuthorModel?> GetAuthor(Guid id);
    Task<AuthorModel?> GetAuthor(string username);
    Task<List<AuthorModel>> GetAuthors(QueryOptions options);
    Task<int> GetAuthorsCount();
    Task<List<PostModel>> GetAuthorPosts(Guid id, QueryOptions options);
    Task<List<PostModel>> GetAuthorPosts(string userName, QueryOptions options);
    Task<int> GetAuthorPostsCount(Guid id);
    Task<int> GetAuthorPostsCount(string userName);
    Task<List<ThreadModel>> GetAuthorThreads(Guid id, QueryOptions options);
    Task<List<ThreadModel>> GetAuthorThreads(string userName, QueryOptions options);
    Task<int> GetAuthorThreadsCount(Guid id);
    Task<int> GetAuthorThreadsCount(string userName);
    Task<int> Save();
}

public class AuthorsRepository : IAuthorsRepository
{
    private readonly BlogContext _blogContext;

    public AuthorsRepository(BlogContext blogContext)
    {
        this._blogContext = blogContext;
    }

    public Task<AuthorModel?> GetAuthor(Guid id)
    {
        return _blogContext.Authors
            .Include(a => a.Profile)
            .SingleOrDefaultAsync(a => a.Id == id);
    }

    public Task<AuthorModel?> GetAuthor(string username)
    {
        return _blogContext.Authors
            .Include(a => a.Profile)
            .SingleOrDefaultAsync(a => a.UserName == username);
    }

    private IQueryable<AuthorModel> AuthorsQuery(string order)
    {
        return _blogContext.Authors
            .AsNoTracking()
            .Include(a => a.Profile)
            .Select(a => new AuthorModel
            {
                Id = a.Id,
                UserName = a.UserName,
                Email = a.Email,
                Profile = new ProfileModel
                {
                    FirstName = a.Profile.FirstName,
                    LastName = a.Profile.LastName,
                    Title = a.Profile.Title,
                    ShortBio = a.Profile.ShortBio,
                    PublicEmail = a.Profile.PublicEmail,
                    AvatarUrl = a.Profile.AvatarUrl
                }
            })
            .AuthorOrder(order);
    }

    public Task<List<AuthorModel>> GetAuthors(QueryOptions options)
    {
        return AuthorsQuery(options.OrderBy)
            .Skip(options.GetSkip())
            .Take(options.GetTake())
            .ToListAsync();
    }

    public Task<int> GetAuthorsCount()
    {
        return _blogContext.Authors.CountAsync();
    }

    private IQueryable<PostModel> PostsQuery(string order)
    {
        return _blogContext.Posts
            .AsNoTracking()
            .Include(p => p.Author)
            .ThenInclude(a => a!.Profile)
            .Select(p => new PostModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Tags = p.Tags,
                AuthorId = p.AuthorId,
                Author = p.Author,
                ThreadId = p.ThreadId,
                Thread = p.Thread,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
            })
            .PostsOrder(order);
    }

    public Task<List<PostModel>> GetAuthorPosts(Guid id, QueryOptions options)
    {
        return PostsQuery(options.OrderBy)
            .Where(p => p.AuthorId == id)
            .Skip(options.GetSkip())
            .Take(options.GetTake())
            .ToListAsync();
    }

    public Task<int> GetAuthorPostsCount(Guid id)
    {
        return _blogContext.Posts.Where(p => p.AuthorId == id).CountAsync();
    }

    public Task<List<PostModel>> GetAuthorPosts(string userName, QueryOptions options)
    {
        return PostsQuery(options.OrderBy)
            .Where(p => p.Author!.UserName == userName)
            .Skip(options.GetSkip())
            .Take(options.GetTake())
            .ToListAsync();
    }

    public Task<int> GetAuthorPostsCount(string userName)
    {
        return _blogContext.Posts
            .Include(p => p.Author)
            .Where(p => p.Author!.UserName == userName)
            .CountAsync();
    }

    private IQueryable<ThreadModel> ThreadsQuery(string order)
    {
        return _blogContext.Threads
            .AsNoTracking()
            .Include(t => t.Author)
            .ThenInclude(a => a!.Profile)
            .ThreadsOrder(order);
    }

    public Task<List<ThreadModel>> GetAuthorThreads(Guid id, QueryOptions options)
    {
        return ThreadsQuery(options.OrderBy)
            .Where(p => p.AuthorId == id)
            .Skip(options.GetSkip())
            .Take(options.GetTake())
            .ToListAsync();
    }

    public Task<int> GetAuthorThreadsCount(Guid id)
    {
        return _blogContext.Threads.Where(p => p.AuthorId == id).CountAsync();
    }

    public Task<List<ThreadModel>> GetAuthorThreads(string userName, QueryOptions options)
    {
        return ThreadsQuery(options.OrderBy)
            .Where(p => p.Author!.UserName == userName)
            .Skip(options.GetSkip())
            .Take(options.GetTake())
            .ToListAsync();
    }

    public Task<int> GetAuthorThreadsCount(string userName)
    {
        return _blogContext.Threads
            .Include(p => p.Author)
            .Where(p => p.Author!.UserName == userName)
            .CountAsync();
    }

    public Task<int> Save()
    {
        return _blogContext.SaveChangesAsync();
    }
}
