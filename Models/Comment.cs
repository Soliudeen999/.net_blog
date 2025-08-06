namespace Blog.Models;

public partial class Comment
{
    public long Id { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }

    public long PostId { get; set; }
    public Post? Post { get; set; }

    public long UserId { get; set; }
    public User? User { get; set; }

    public long? ParentId { get; set; }
    public Comment? Parent { get; set; }
    public ICollection<Comment> Replies { get; set; } = [];
}