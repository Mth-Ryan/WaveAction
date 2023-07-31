using Microsoft.EntityFrameworkCore;
using WaveActionApi.Data;
using WaveActionApi.Models;

namespace WaveActionApi.Repositories;

public interface IThreadsRepository
{
    Task<ThreadModel?> GetThread(Guid id);
    Task<ThreadModel?> GetThread(string titleSlug);
    Task<List<ThreadModel>> GetThreads(QueryOptions options);
    Task<List<PostModel>> GetThreadPosts(Guid id, QueryOptions options);
    Task<List<PostModel>> GetThreadPosts(string titleSlug, QueryOptions options);
    Task<int> GetThreadPostsCount(Guid id);
    Task<int> GetThreadPostsCount(string titleSlug);
    Task<int> GetThreadsCount();
    Task<int> Save();
    Task<int> Add(ThreadModel thread);
    Task<int> Delete(ThreadModel thread);
}

public class ThreadsRepository : IThreadsRepository
{
    private readonly BlogContext _blogContext;

    public ThreadsRepository(BlogContext blogContext)
    {
        this._blogContext = blogContext;
    }

    public Task<ThreadModel?> GetThread(Guid id)
    {
        return _blogContext.Threads
            .Include(p => p.Author).ThenInclude(a => a!.Profile)
            .SingleOrDefaultAsync(t => t.Id == id);
    }

    public Task<ThreadModel?> GetThread(string titleSlug)
    {
        return _blogContext.Threads
            .Include(p => p.Author).ThenInclude(a => a!.Profile)
            .SingleOrDefaultAsync(t => t.TitleSlug == titleSlug);
    }

    private IQueryable<ThreadModel> ThreadsQuery(string order)
    {
        var query = _blogContext.Threads
            .AsNoTracking()
            .Include(t => t.Author)
            .ThenInclude(a => a!.Profile)
            .Select(t => t);

        switch (order)
        {
            case "title.asc":
                query = query.OrderBy(p => p.Title);
                break;

            case "title.desc":
                query = query.OrderByDescending(p => p.Title);
                break;

            case "updatedAt.asc":
                query = query.OrderBy(p => p.UpdatedAt);
                break;

            case "updatedAt.desc":
                query = query.OrderByDescending(p => p.UpdatedAt);
                break;

            case "createdAt.asc":
                query = query.OrderBy(p => p.CreatedAt);
                break;

            default:
                query = query.OrderByDescending(p => p.CreatedAt);
                break;
        }

        return query;
    }

    public Task<List<ThreadModel>> GetThreads(QueryOptions options)
    {
        return ThreadsQuery(options.OrderBy)
            .Skip(options.GetSkip())
            .Take(options.GetTake())
            .ToListAsync();
    }

    private IQueryable<PostModel> PostsQuery(string order)
    {
        var query = _blogContext.Posts
            .AsNoTracking()
            .Include(p => p.Author)
            .ThenInclude(a => a!.Profile)
            .Include(p => p.Thread)
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
            });

        switch (order)
        {
            case "title.asc":
                query = query.OrderBy(p => p.Title);
                break;

            case "title.desc":
                query = query.OrderByDescending(p => p.Title);
                break;

            case "updatedAt.asc":
                query = query.OrderBy(p => p.UpdatedAt);
                break;

            case "updatedAt.desc":
                query = query.OrderByDescending(p => p.UpdatedAt);
                break;

            case "createdAt.asc":
                query = query.OrderBy(p => p.CreatedAt);
                break;

            default:
                query = query.OrderByDescending(p => p.CreatedAt);
                break;
        }

        return query;
    }

    public Task<List<PostModel>> GetThreadPosts(Guid id, QueryOptions options)
    {
        return PostsQuery(options.OrderBy)
            .Where(p => p.ThreadId == id)
            .Skip(options.GetSkip())
            .Take(options.GetTake())
            .ToListAsync();
    }

    public Task<int> GetThreadPostsCount(Guid id)
    {
        return _blogContext.Posts.Where(p => p.ThreadId == id).CountAsync();
    }

    public Task<List<PostModel>> GetThreadPosts(string titleSlug, QueryOptions options)
    {
        return PostsQuery(options.OrderBy)
            .Where(p => p.Thread!.TitleSlug == titleSlug)
            .Skip(options.GetSkip())
            .Take(options.GetTake())
            .ToListAsync();
    }

    public Task<int> GetThreadPostsCount(string titleSlug)
    {
        return _blogContext.Posts
            .Include(p => p.Thread)
            .Where(p => p.Thread!.TitleSlug == titleSlug)
            .CountAsync();
    }

    public Task<int> GetThreadsCount()
    {
        return _blogContext.Threads.CountAsync();
    }

    public Task<int> Save()
    {
        return _blogContext.SaveChangesAsync();
    }

    public Task<int> Add(ThreadModel thread)
    {
        _blogContext.Threads.Add(thread);
        return Save();
    }

    public Task<int> Delete(ThreadModel thread)
    {
        _blogContext.Threads.Remove(thread);
        return Save();
    }
}
