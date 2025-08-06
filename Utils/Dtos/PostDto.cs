namespace Blog.Utils.Dtos;

public class PostDto
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Slug { get; set; }
    public string? Content { get; set; }
    public string? PublisherName { get; set; }
    public string? MediaPath { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? UserName { get; set; }
    public List<string> Tags { get; set; } = [];
}
