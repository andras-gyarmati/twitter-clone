using Microsoft.EntityFrameworkCore;
using TwitterClone.Models;

namespace TwitterClone;

public class TwitterCloneDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<Tweet> Tweets { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<UserUser> UserUsers { get; set; }

    public TwitterCloneDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite(_configuration.GetConnectionString("TwitterCloneDatabase"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tweet>().HasOne(t => t.Author).WithMany(a => a.Tweets).HasForeignKey(x => x.AuthorId);
        modelBuilder.Entity<Tweet>().HasOne(t => t.InReplyTo).WithMany(t => t.Replies).HasForeignKey(t => t.InReplyToId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<User>()
            .HasMany(u => u.LikedTweets)
            .WithMany(t => t.LikedByUsers)
            .UsingEntity<Dictionary<string, object>>(
                "UserTweetLikes",
                j => j.HasOne<Tweet>().WithMany().HasForeignKey("TweetId"),
                j => j.HasOne<User>().WithMany().HasForeignKey("UserId")
            );
        modelBuilder.Entity<UserUser>().HasKey(u => new { u.FollowedId, u.FollowerId });
        modelBuilder.Entity<UserUser>().HasOne(u => u.Follower).WithMany(u => u.Following).HasForeignKey(u => u.FollowerId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<UserUser>().HasOne(u => u.Followed).WithMany(u => u.Followers).HasForeignKey(u => u.FollowedId).OnDelete(DeleteBehavior.NoAction);
    }
}
