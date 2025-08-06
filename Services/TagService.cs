using Blog.Models;
using Blog.Utils.Dtos;

namespace Blog.Services;

public class TagService
{
    private readonly AppDbContext _dbContext;

    public TagService(AppDbContext context)
    {
        _dbContext = context;
    }

    public List<TagDto> GetAll()
    {
        return _dbContext.Tags
            .Select(t => new TagDto
            {
                Id = t.Id,
                Name = t.Name
            })
            .ToList();
    }

    public TagDto? Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        var existingTag = _dbContext.Tags.FirstOrDefault(t => t.Name == name);
        if (existingTag != null)
            return null; // Tag already exists

        var tag = new Tag { Name = name };
        _dbContext.Tags.Add(tag);
        _dbContext.SaveChanges();

        return new TagDto
        {
            Id = tag.Id,
            Name = tag.Name
        };
    }

    public bool Delete(int id)
    {
        var tag = _dbContext.Tags.Find(id);
        if (tag == null) return false;

        _dbContext.Tags.Remove(tag);
        _dbContext.SaveChanges();
        return true;
    }
} 