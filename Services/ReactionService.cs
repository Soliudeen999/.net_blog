using Blog.Models;
using Blog.Utils.Requests;
using Blog.Utils.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services;

public class ReactionService
{
    private readonly AppDbContext _dbContext;

    public ReactionService(AppDbContext context)
    {
        _dbContext = context;
    }

    private IQueryable<Reaction> ReactionQuery => _dbContext.Reactions
        .Include(r => r.User)
        .OrderByDescending(r => r.CreatedAt);

    public List<ReactionDto> GetByPostId(int postId)
    {
        return ReactionQuery
            .Where(r => r.PostId == postId)
            .Select(r => new ReactionDto
            {
                Id = r.Id,
                Type = r.Type,
                CreatedAt = r.CreatedAt,
                UserName = r.User != null ? r.User.Name : null,
                UserEmail = r.User != null ? r.User.Email : null
            })
            .ToList();
    }

    public ReactionDto? Create(int postId, CreateReactionRequest request, string userEmail)
    {
        var post = _dbContext.Posts.FirstOrDefault(p => p.Id == postId);
        if (post == null) return null;

        var user = _dbContext.Users.FirstOrDefault(u => u.Email == userEmail);
        if (user == null) return null;

        // Check if user already has a reaction on this post
        var existingReaction = _dbContext.Reactions
            .FirstOrDefault(r => r.PostId == postId && r.UserId == user.Id);

        if (existingReaction != null)
        {
            // Update existing reaction
            existingReaction.Type = request.Type;
            existingReaction.CreatedAt = DateTime.UtcNow;
            _dbContext.SaveChanges();

            return new ReactionDto
            {
                Id = existingReaction.Id,
                Type = existingReaction.Type,
                CreatedAt = existingReaction.CreatedAt,
                UserName = user.Name,
                UserEmail = user.Email
            };
        }

        // Create new reaction
        var reaction = new Reaction
        {
            Type = request.Type,
            PostId = postId,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Reactions.Add(reaction);
        _dbContext.SaveChanges();

        return new ReactionDto
        {
            Id = reaction.Id,
            Type = reaction.Type,
            CreatedAt = reaction.CreatedAt,
            UserName = user.Name,
            UserEmail = user.Email
        };
    }

    public bool Delete(int reactionId, string userEmail)
    {
        var reaction = _dbContext.Reactions.FirstOrDefault(r => r.Id == reactionId);
        if (reaction == null) return false;

        var user = _dbContext.Users.FirstOrDefault(u => u.Email == userEmail);
        if (user == null || reaction.UserId != user.Id)
            return false; // Unauthorized

        _dbContext.Reactions.Remove(reaction);
        _dbContext.SaveChanges();
        return true;
    }

    public ReactionSummaryDto GetReactionSummary(int postId)
    {
        var reactions = _dbContext.Reactions
            .Where(r => r.PostId == postId)
            .GroupBy(r => r.Type)
            .Select(g => new ReactionCountDto
            {
                Type = g.Key,
                Count = g.Count()
            })
            .ToList();

        var totalReactions = reactions.Sum(r => r.Count);

        return new ReactionSummaryDto
        {
            PostId = postId,
            TotalReactions = totalReactions,
            Reactions = reactions
        };
    }
} 