using Blog.Models;
using Blog.Utils.Requests;
using Blog.Utils.Dtos;
using Microsoft.EntityFrameworkCore;
using Blog.Utils;

namespace Blog.Services;

public class CommentService
{
    private readonly AppDbContext _dbContext;

    public CommentService(AppDbContext context)
    {
        _dbContext = context;
    }

    private IQueryable<Comment> CommentQuery => _dbContext.Comments
        .Include(c => c.User)
        .Include(c => c.Replies)
        .OrderByDescending(c => c.CreatedAt);

    public List<CommentDto> GetByPostId(int postId, int page, int perPage)
    {
        var comments = CommentQuery
            .Where(c => c.PostId == postId && c.ParentId == null) // Only top-level comments
            .ApplyPagination(page, perPage)
            .ToList();

        return comments.Select(c => new CommentDto
        {
            Id = c.Id,
            Content = c.Content,
            CreatedAt = c.CreatedAt,
            UserName = c.User?.Name,
            UserEmail = c.User?.Email,
            Replies = c.Replies.Select(r => new CommentDto
            {
                Id = r.Id,
                Content = r.Content,
                CreatedAt = r.CreatedAt,
                UserName = r.User?.Name,
                UserEmail = r.User?.Email
            }).ToList()
        }).ToList();
    }

    public CommentDto? Create(int postId, CreateCommentRequest request, string userEmail)
    {
        var post = _dbContext.Posts.FirstOrDefault(p => p.Id == postId);
        if (post == null) return null;

        var user = _dbContext.Users.FirstOrDefault(u => u.Email == userEmail);
        if (user == null) return null;

        var comment = new Comment
        {
            Content = request.Content,
            PostId = postId,
            UserId = user.Id,
            ParentId = request.ParentId,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Comments.Add(comment);
        _dbContext.SaveChanges();

        return new CommentDto
        {
            Id = comment.Id,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            UserName = user.Name,
            UserEmail = user.Email
        };
    }

    public CommentDto? Update(int id, UpdateCommentRequest request, string userEmail)
    {
        var comment = _dbContext.Comments
            .Include(c => c.User)
            .FirstOrDefault(c => c.Id == id);

        if (comment == null) return null;

        var user = _dbContext.Users.FirstOrDefault(u => u.Email == userEmail);
        if (user == null || comment.UserId != user.Id)
            return null; // Unauthorized

        if (!string.IsNullOrEmpty(request.Content))
            comment.Content = request.Content;

        _dbContext.SaveChanges();

        return new CommentDto
        {
            Id = comment.Id,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            UserName = comment.User?.Name,
            UserEmail = comment.User?.Email
        };
    }

    public bool Delete(int id, string userEmail)
    {
        var comment = _dbContext.Comments.FirstOrDefault(c => c.Id == id);
        if (comment == null) return false;

        var user = _dbContext.Users.FirstOrDefault(u => u.Email == userEmail);
        if (user == null || comment.UserId != user.Id)
            return false; // Unauthorized

        _dbContext.Comments.Remove(comment);
        _dbContext.SaveChanges();
        return true;
    }
} 