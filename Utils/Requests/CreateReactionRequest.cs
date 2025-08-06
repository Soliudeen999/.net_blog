using System.ComponentModel.DataAnnotations;
using Blog.Models;

namespace Blog.Utils.Requests;

public class CreateReactionRequest
{
    [Required]
    public ReactionType Type { get; set; }
} 