namespace Blog.Utils.Dtos;

public class CommentDto
{
    public long Id { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? UserName { get; set; }
    public string? UserEmail { get; set; }
    public List<CommentDto> Replies { get; set; } = new();
} 