using Blog.Models;
using Blog.Services;

namespace Blog.Utils;

public static class DatabaseSeeder
{
    public static void SeedDatabase(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var authService = scope.ServiceProvider.GetRequiredService<AuthService>();

        // Check if data already exists
        if (dbContext.Users.Any())
            return;

        // Create sample users
        var users = new List<User>
        {
            new User
            {
                Name = "John Doe",
                Email = "john@example.com",
                Password = authService.HashPassword(new User(), "password"),
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Name = "Jane Smith",
                Email = "jane@example.com",
                Password = authService.HashPassword(new User(), "password"),
                CreatedAt = DateTime.UtcNow
            }
        };

        dbContext.Users.AddRange(users);
        dbContext.SaveChanges();

        // Create sample tags
        var tags = new List<Tag>
        {
            new Tag { Name = "Technology" },
            new Tag { Name = "Programming" },
            new Tag { Name = "Web Development" },
            new Tag { Name = "C#" },
            new Tag { Name = "ASP.NET Core" }
        };

        dbContext.Tags.AddRange(tags);
        dbContext.SaveChanges();

        // Create sample posts
        var posts = new List<Post>
        {
            new Post
            {
                Title = "Getting Started with ASP.NET Core",
                Slug = "getting-started-with-aspnet-core",
                Content = "ASP.NET Core is a cross-platform, high-performance, open-source framework for building modern, cloud-enabled, Internet-connected applications.",
                CategoryName = "Programming",
                PublisherName = "John Doe",
                Status = Status.Published,
                UserId = users[0].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                PublishedAt = DateTime.UtcNow.AddDays(-5)
            },
            new Post
            {
                Title = "Entity Framework Core Best Practices",
                Slug = "entity-framework-core-best-practices",
                Content = "Entity Framework Core is Microsoft's modern object-database mapper for .NET. It supports LINQ queries, change tracking, updates, and schema migrations.",
                CategoryName = "Database",
                PublisherName = "Jane Smith",
                Status = Status.Published,
                UserId = users[1].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                PublishedAt = DateTime.UtcNow.AddDays(-3)
            }
        };

        dbContext.Posts.AddRange(posts);
        dbContext.SaveChanges();

        // Create post-tag relationships
        var postTags = new List<PostTag>
        {
            new PostTag { PostId = (int)posts[0].Id, TagId = tags[0].Id },
            new PostTag { PostId = (int)posts[0].Id, TagId = tags[1].Id },
            new PostTag { PostId = (int)posts[0].Id, TagId = tags[2].Id },
            new PostTag { PostId = (int)posts[1].Id, TagId = tags[0].Id },
            new PostTag { PostId = (int)posts[1].Id, TagId = tags[1].Id }
        };

        dbContext.PostTag.AddRange(postTags);
        dbContext.SaveChanges();

        // Create sample comments
        var comments = new List<Comment>
        {
            new Comment
            {
                Content = "Great article! Very helpful for beginners.",
                PostId = (int)posts[0].Id,
                UserId = users[1].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-4)
            },
            new Comment
            {
                Content = "Thanks for sharing these insights!",
                PostId = (int)posts[1].Id,
                UserId = users[0].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            }
        };

        dbContext.Comments.AddRange(comments);
        dbContext.SaveChanges();
    }
} 