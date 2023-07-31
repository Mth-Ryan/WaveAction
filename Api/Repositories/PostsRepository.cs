using Microsoft.EntityFrameworkCore;
using WaveActionApi.Data;
using WaveActionApi.Models;

namespace WaveActionApi.Repositories;

public interface IPostsRepository
{
    Task<PostModel?> GetPost(Guid id);
    Task<PostModel?> GetPost(string titleSlug);
    Task<List<PostModel>> GetPosts(QueryOptions options);
    Task<int> GetPostsCount();
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

    private IQueryable<PostModel> PostsQuery(string order)
    {
        var query = _blogContext.Posts
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

    public Task<List<PostModel>> GetPosts(QueryOptions options)
    {
        var query = PostsQuery(options.OrderBy);

        // pagination
        query.Skip(options.GetSkip())
            .Take(options.GetTake());

        return query.ToListAsync();
    }

    public Task<int> GetPostsCount()
    {
        return _blogContext.Posts.CountAsync();
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
