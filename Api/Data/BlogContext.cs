using Microsoft.EntityFrameworkCore;
using WaveActionApi.Models;

namespace WaveActionApi.Data;

public class BlogContext : DbContext
{
    private readonly IConfiguration _config;

    public BlogContext(IConfiguration configuration)
    {
        _config = configuration;
    }

    public required DbSet<AuthorModel> Authors { get; set; }
    public required DbSet<ProfileModel> Profiles { get; set; }
    public required DbSet<ThreadModel> Threads { get; set; }
    public required DbSet<PostModel> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(_config.GetConnectionString("Postgres"));
}