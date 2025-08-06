using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { set; get; }

    public DbSet<Post> Posts { set; get; }

    public DbSet<Comment> Comments { set; get; }

    public DbSet<Reaction> Reactions { set; get; }

    public DbSet<Tag> Tags { set; get; }

    public DbSet<PostTag> PostTag { set; get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostTag>()
            .HasKey(pt => new { pt.PostId, pt.TagId });

        modelBuilder.Entity<PostTag>()
            .HasOne(pt => pt.Post)
            .WithMany(p => p.PostTags)
            .HasForeignKey(pt => pt.PostId);

        modelBuilder.Entity<PostTag>()
            .HasOne(pt => pt.Tag)
            .WithMany()
            .HasForeignKey(pt => pt.TagId);

        modelBuilder.Entity<Post>()
            .Property(p => p.Status)
            .HasConversion<string>();
    }
}