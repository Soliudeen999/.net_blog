using Blog.Models;
using Blog.Utils.Dtos;
using Microsoft.EntityFrameworkCore;
using Blog.Utils;
using Blog.Utils.Requests;

namespace Blog.Services;

public class PostService
{
    private readonly AppDbContext _dbContext;

    public PostService(AppDbContext context)
    {
        _dbContext = context;
    }

    private IQueryable<Post> PostQuery => _dbContext.Posts
        .Include(p => p.User)
        .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
        .OrderByDescending(p => p.CreatedAt);

    public List<PostDto> GetAll(
        int page,
        int perPage,
        string? search = null,
        int? userId = null,
        int? tagId = null,
        Status? status = null,
        string? orderBy = null

    ){
        var query = PostQuery
            .ApplyPostFilters(search, userId, tagId, status)
            .ApplySorting(orderBy)
            .ApplyPagination(page, perPage);

        return query.Select(p => new PostDto
        {
            Id = p.Id,
            Title = p.Title,
            Slug = p.Slug,
            Content = p.Content,
            PublisherName = p.PublisherName,
            MediaPath = p.MediaPath,
            CreatedAt = p.CreatedAt,
            PublishedAt = p.PublishedAt,
            UserName = p.User != null ? p.User.Name : null,
            Tags = p.PostTags.Select(pt => pt.Tag.Name).Where(name => name != null).Cast<string>().ToList()
        }).ToList();
    }

    public PostDto? FindById(int id)
    {
        var post = PostQuery.FirstOrDefault(p => p.Id == id);
        if (post == null) return null;

        return new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Slug = post.Slug,
            Content = post.Content,
            PublisherName = post.PublisherName,
            MediaPath = post.MediaPath,
            CreatedAt = post.CreatedAt,
            PublishedAt = post.PublishedAt,
            UserName = post.User?.Name,
            Tags = post.PostTags.Select(pt => pt.Tag.Name).Where(name => name != null).Cast<string>().ToList()
        };
    }

    public PostDto? FindBySlug(string slug)
    {
        var post = PostQuery.FirstOrDefault(p => p.Slug == slug);
        if (post == null) return null;

        return new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Slug = post.Slug,
            Content = post.Content,
            PublisherName = post.PublisherName,
            MediaPath = post.MediaPath,
            CreatedAt = post.CreatedAt,
            PublishedAt = post.PublishedAt,
            UserName = post.User?.Name,
            Tags = post.PostTags.Select(pt => pt.Tag.Name).Where(name => name != null).Cast<string>().ToList()
        };
    }

    public PostDto Create(CreatePostRequest request, string userEmail)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Email == userEmail);
        if (user == null)
            throw new InvalidOperationException("User not found");

        var post = new Post
        {
            Title = request.Title,
            Slug = request.Slug ?? GenerateSlug(request.Title),
            Content = request.Content,
            CategoryName = request.CategoryName,
            MediaPath = request.MediaPath,
            Status = request.Status,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            PublishedAt = request.Status == Status.Published ? DateTime.UtcNow : null
        };

        _dbContext.Posts.Add(post);
        _dbContext.SaveChanges();

        // Handle tags
        if (request.Tags.Any())
        {
            foreach (var tagName in request.Tags)
            {
                var tag = _dbContext.Tags.FirstOrDefault(t => t.Name == tagName);
                if (tag == null)
                {
                    tag = new Tag { Name = tagName };
                    _dbContext.Tags.Add(tag);
                    _dbContext.SaveChanges();
                }

                var postTag = new PostTag
                {
                    PostId = (int)post.Id,
                    TagId = tag.Id
                };
                _dbContext.PostTag.Add(postTag);
            }
            _dbContext.SaveChanges();
        }

        return FindById((int)post.Id)!;
    }

    public PostDto? Update(int id, UpdatePostRequest request, string userEmail)
    {
        var post = _dbContext.Posts
            .Include(p => p.PostTags)
            .FirstOrDefault(p => p.Id == id);

        if (post == null) return null;

        var user = _dbContext.Users.FirstOrDefault(u => u.Email == userEmail);
        if (user == null || post.UserId != user.Id)
            return null; // Unauthorized

        // Update fields if provided
        if (!string.IsNullOrEmpty(request.Title))
            post.Title = request.Title;

        if (!string.IsNullOrEmpty(request.Slug))
            post.Slug = request.Slug;

        if (request.Content != null)
            post.Content = request.Content;

        if (request.CategoryName != null)
            post.CategoryName = request.CategoryName;

        if (request.MediaPath != null)
            post.MediaPath = request.MediaPath;

        if (request.PublisherName != null)
            post.PublisherName = request.PublisherName;

        if (request.Status.HasValue)
        {
            post.Status = request.Status.Value;
            if (request.Status.Value == Status.Published && post.PublishedAt == null)
                post.PublishedAt = DateTime.UtcNow;
        }

        // Handle tags if provided
        if (request.Tags != null)
        {
            // Remove existing tags
            _dbContext.PostTag.RemoveRange(post.PostTags);

            // Add new tags
            foreach (var tagName in request.Tags)
            {
                var tag = _dbContext.Tags.FirstOrDefault(t => t.Name == tagName);
                if (tag == null)
                {
                    tag = new Tag { Name = tagName };
                    _dbContext.Tags.Add(tag);
                    _dbContext.SaveChanges();
                }

                var postTag = new PostTag
                {
                    PostId = (int)post.Id,
                    TagId = tag.Id
                };
                _dbContext.PostTag.Add(postTag);
            }
        }

        _dbContext.SaveChanges();
        return FindById(id);
    }

    public bool Delete(int id, string userEmail)
    {
        var post = _dbContext.Posts.FirstOrDefault(p => p.Id == id);
        if (post == null) return false;

        var user = _dbContext.Users.FirstOrDefault(u => u.Email == userEmail);
        if (user == null || post.UserId != user.Id)
            return false; // Unauthorized

        _dbContext.Posts.Remove(post);
        _dbContext.SaveChanges();
        return true;
    }

    private string GenerateSlug(string title)
    {
        return title.ToLower()
            .Replace(" ", "-")
            .Replace("_", "-")
            .Replace(".", "")
            .Replace(",", "")
            .Replace("!", "")
            .Replace("?", "")
            .Replace(":", "")
            .Replace(";", "");
    }
}
