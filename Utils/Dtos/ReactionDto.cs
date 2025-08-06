using Blog.Models;

namespace Blog.Utils.Dtos;

public class ReactionDto
{
    public long Id { get; set; }
    public ReactionType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? UserName { get; set; }
    public string? UserEmail { get; set; }
} 