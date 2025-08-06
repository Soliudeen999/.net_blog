using System.ComponentModel.DataAnnotations;
using Blog.Models;

namespace Blog.Utils.Requests;

public class CreatePostRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;



    public string? Content { get; set; }

    [MaxLength(100)]
    public string? CategoryName { get; set; }

    [MaxLength(500)]
    public string? MediaPath { get; set; }

    public Status Status { get; set; } = Status.Draft;

    public List<string> Tags { get; set; } = new();
} 