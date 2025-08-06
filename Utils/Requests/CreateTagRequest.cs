using System.ComponentModel.DataAnnotations;

namespace Blog.Utils.Requests;

public class CreateTagRequest
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
} 