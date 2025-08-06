using System.ComponentModel.DataAnnotations;

namespace Blog.Utils.Requests;

public class UpdateCommentRequest
{
    [Required]
    [MaxLength(1000)]
    public string Content { get; set; } = string.Empty;
} 