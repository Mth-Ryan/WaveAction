using Microsoft.EntityFrameworkCore;
using WaveActionApi.Data;
using WaveActionApi.Models;

namespace WaveActionApi.Repositories;

public interface IPostsRepository
{
    Task<PostModel?> GetPost(Guid id);
    Task<PostModel?> GetPost(string titleSlug);
    Task<List<PostModel>> GetPosts(QueryOptions options);
    Task<int> GetPostsCount(QueryOptions options);
    Task<int> Save();
    Task<int> Add(PostModel post);
    Task<int> Delete(PostModel post);
}

public class PostsRepository : IPostsRepository
{
    private readonly BlogContext _blogContext;

    public PostsRepository(BlogContext blogContext)
    {
        this._blogContext = blogContext;
    }

    public Task<PostModel?> GetPost(Guid id)
    {
        return _blogContext.Posts
            .Include(p => p.Author).ThenInclude(a => a!.Profile)
            .Include(p => p.Thread)
            .SingleOrDefaultAsync(t => t.Id == id);
    }

    public Task<PostModel?> GetPost(string titleSlug)
    {
        return _blogContext.Posts
            .Include(p => p.Author).ThenInclude(a => a!.Profile)
            .Include(p => p.Thread)
            .SingleOrDefaultAsync(t => t.TitleSlug == titleSlug);
    }

    private IQueryable<PostModel> PostsQuery(string? search, string order)
    {
        return _blogContext.Posts
            .AsNoTracking()
            .Include(t => t.Author).ThenInclude(a => a!.Profile)
            .Include(p => p.Thread)
            .Select(p => new PostModel
            {
                Id = p.Id,
                Title = p.Title,
                TitleSlug = p.TitleSlug,
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

    public Task<List<PostModel>> GetPosts(QueryOptions options)
    {
        return PostsQuery(options.SimpleSearch, options.OrderBy)
            .Paginate(options.Page, options.PageSize)
            .ToListAsync();
    }

    public Task<int> GetPostsCount(QueryOptions options)
    {
        return _blogContext.Posts
            .SimpleSearch(options.SimpleSearch)
            .CountAsync();
    }

    public Task<int> Save()
    {
        return _blogContext.SaveChangesAsync();
    }

    public Task<int> Add(PostModel post)
    {
        _blogContext.Posts.Add(post);
        return Save();
    }

    public Task<int> Delete(PostModel post)
    {
        _blogContext.Posts.Remove(post);
        return Save();
    }
}
