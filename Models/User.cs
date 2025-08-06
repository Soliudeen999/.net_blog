using System.Collections.ObjectModel;

namespace Blog.Models;

public partial class User
{
    public long Id { get; set; }
    public string? Name { get; set; }

    public string? Email { get; set; }
    public string? Password { get; set; }

    public DateTime CreatedAt { get; set; }
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}