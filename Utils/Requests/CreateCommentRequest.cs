using System.ComponentModel.DataAnnotations;

namespace Blog.Utils.Requests;

public class CreateCommentRequest
{
    [Required]
    [MaxLength(1000)]
    public string Content { get; set; } = string.Empty;

    public int? ParentId { get; set; } // For replies
} 