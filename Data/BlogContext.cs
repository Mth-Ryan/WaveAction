using Microsoft.EntityFrameworkCore;
using WaveAction.Models;

namespace WaveAction.Data;

class BlogContext : DbContext
{
    private IConfiguration _config;

    public BlogContext(IConfiguration configuration)
    {
        _config = configuration;
    }

    public virtual DbSet<AuthorModel> Authors { get; set; }
    public virtual DbSet<ProfileModel> Profiles { get; set; }
    public virtual DbSet<ThreadModel> Threads { get; set; }
    public virtual DbSet<PostModel> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        => optionsBuilder.UseNpgsql(_config.GetConnectionString("Postgres"));
}