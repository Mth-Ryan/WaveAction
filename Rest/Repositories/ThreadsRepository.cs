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
    Task<int> GetThreadPostsCount(Guid id, QueryOptions options);
    Task<int> GetThreadPostsCount(string titleSlug, QueryOptions options);
    Task<int> GetThreadsCount(QueryOptions options);
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

    private IQueryable<ThreadModel> ThreadsQuery(string? search, string order)
    {
        return _blogContext.Threads
            .AsNoTracking()
            .Include(t => t.Author)
            .ThenInclude(a => a!.Profile)
            .Select(t => new ThreadModel
            {
                Id = t.Id,
                ThumbnailUrl = t.ThumbnailUrl,
                Title = t.Title,
                Description = t.Description,
                AuthorId = t.AuthorId,
                Author = t.Author,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
            })
            .SimpleSearch(search)
            .ThreadsOrder(order);
    }

    public Task<List<ThreadModel>> GetThreads(QueryOptions options)
    {
        return ThreadsQuery(options.SimpleSearch, options.OrderBy)
            .Paginate(options.Page, options.PageSize)
            .ToListAsync();
    }

    private IQueryable<PostModel> PostsQuery(string? search, string order)
    {
        return _blogContext.Posts
            .AsNoTracking()
            .Include(p => p.Author)
            .ThenInclude(a => a!.Profile)
            .Include(p => p.Thread)
            .Select(p => new PostModel
            {
                Id = p.Id,
                ThumbnailUrl = p.ThumbnailUrl,
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
            .SimpleSearch(search)
            .PostsOrder(order);
    }

    public Task<List<PostModel>> GetThreadPosts(Guid id, QueryOptions options)
    {
        return PostsQuery(options.SimpleSearch, options.OrderBy)
            .Where(p => p.ThreadId == id)
            .Paginate(options.Page, options.PageSize)
            .ToListAsync();
    }

    public Task<int> GetThreadPostsCount(Guid id, QueryOptions options)
    {
        return _blogContext.Posts
            .SimpleSearch(options.SimpleSearch)
            .Where(p => p.ThreadId == id)
            .CountAsync();
    }

    public Task<List<PostModel>> GetThreadPosts(string titleSlug, QueryOptions options)
    {
        return PostsQuery(options.SimpleSearch, options.OrderBy)
            .Where(p => p.Thread!.TitleSlug == titleSlug)
            .Paginate(options.Page, options.PageSize)
            .ToListAsync();
    }

    public Task<int> GetThreadPostsCount(string titleSlug, QueryOptions options)
    {
        return _blogContext.Posts
            .Include(p => p.Thread)
            .SimpleSearch(options.SimpleSearch)
            .Where(p => p.Thread!.TitleSlug == titleSlug)
            .CountAsync();
    }

    public Task<int> GetThreadsCount(QueryOptions options)
    {
        return _blogContext.Threads
            .SimpleSearch(options.SimpleSearch)
            .CountAsync();
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
