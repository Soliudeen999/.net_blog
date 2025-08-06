namespace Blog.Models;

public class PostTag
{
    public long PostId { get; set; }
    public Post Post { get; set; } = default!;
    public long TagId { get; set; }
    public Tag Tag { get; set; } = default!;
}
