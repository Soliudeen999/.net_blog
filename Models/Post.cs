using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models;

public class Post
{
    [Key]
    public long Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Slug { get; set; }

    public string? Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? PublishedAt { get; set; }

    [Required]
    public Status Status { get; set; } = Status.Draft;

    public long? UserId { get; set; }

    public User? User { get; set; }

    public string? CategoryName { get; set; }

    [MaxLength(500)]
    public string? MediaPath { get; set; }

    [MaxLength(100)]
    public string? PublisherName { get; set; }

    public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
    
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
}

public enum Status
{
    Draft = 0,
    Published = 1,
    Scheduled = 2
}
