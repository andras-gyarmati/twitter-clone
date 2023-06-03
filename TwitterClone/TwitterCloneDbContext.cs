using Microsoft.EntityFrameworkCore;
using TwitterClone.Models;

namespace TwitterClone;

public class TwitterCloneDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<Tweet> Tweets { get; set; }
    public DbSet<User> Users { get; set; }
    public string DbPath { get; }

    public TwitterCloneDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "twitter-clone.db");
        // Console.WriteLine($"DbPath: {DbPath}");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        // options.UseSqlite($"Data Source={DbPath}");
        options.UseSqlite(_configuration.GetConnectionString("TwitterCloneDatabase"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tweet>().HasOne(t => t.Author).WithMany(a => a.Tweets).HasForeignKey(x => x.AuthorId);
        modelBuilder.Entity<User>().HasMany(u => u.Tweets).WithOne(t => t.Author).HasForeignKey(x => x.AuthorId);
    }
}
