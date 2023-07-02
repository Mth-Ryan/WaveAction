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

    private IQueryable<PostModel> PostsQuery()
    {
        return _blogContext.Posts
            .AsNoTracking()
            .Include(t => t.Author).ThenInclude(a => a!.Profile)
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
            })
            .OrderByDescending(t => t.CreatedAt);
    }

    public Task<List<PostModel>> GetPosts(QueryOptions options)
    {
        return PostsQuery().Skip(options.GetSkip()).Take(options.GetTake()).ToListAsync();
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