namespace Blog.Models;

public partial class Reaction
{
    public long Id { get; set; }

    public long UserId { get; set; }
    public User? User { get; set; } = default;

    public ReactionType Type { get; set; }

    public long PostId { get; set; }
    public Post? Post { get; set; } = default;

    public DateTime CreatedAt { get; set; }
}

public enum ReactionType
{
    Like, Dislike, Love, Laugh, Sad, Angry, Wow, Care
};
