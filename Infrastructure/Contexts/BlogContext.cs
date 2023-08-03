using Microsoft.EntityFrameworkCore;
using WaveAction.Domain.Models;

namespace WaveAction.Infrastructure.Contexts;

public class BlogContext : DbContext
{

    public BlogContext(DbContextOptions<BlogContext> options) : base(options) { }

    public required DbSet<AuthorModel> Authors { get; set; }
    public required DbSet<ProfileModel> Profiles { get; set; }
    public required DbSet<ThreadModel> Threads { get; set; }
    public required DbSet<PostModel> Posts { get; set; }
}
