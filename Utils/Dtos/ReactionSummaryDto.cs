using Blog.Models;

namespace Blog.Utils.Dtos;

public class ReactionSummaryDto
{
    public long PostId { get; set; }
    public int TotalReactions { get; set; }
    public List<ReactionCountDto> Reactions { get; set; } = new();
}

public class ReactionCountDto
{
    public ReactionType Type { get; set; }
    public int Count { get; set; }
} 