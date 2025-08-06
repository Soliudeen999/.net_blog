using Blog.Models;

namespace Blog.Services;

public class UserService(AppDbContext _appDbContext)
{
    private IQueryable<User> UserQuery => _appDbContext.Users;

    public List<User> GetAll()
    {
        return [.. UserQuery];
    }

    public User? GetById(int id)
    {
        return _appDbContext.Users.Find(id);
    }

    public User? GetByEmail(string email)
    {
        return _appDbContext.Users.FirstOrDefault(u => u.Email == email);
    }

    public User Create(User user)
    {
        user.CreatedAt = DateTime.UtcNow;
        _appDbContext.Users.Add(user);
        _appDbContext.SaveChanges();
        return user;
    }

    public User? Update(int id, User updatedUser)
    {
        var existingUser = _appDbContext.Users.Find(id);
        if (existingUser == null) return null;

        existingUser.Name = updatedUser.Name;
        existingUser.Email = updatedUser.Email;
        existingUser.Password = updatedUser.Password;

        _appDbContext.SaveChanges();
        return existingUser;
    }

    public bool Delete(int id)
    {
        var user = _appDbContext.Users.Find(id);
        if (user == null) return false;

        _appDbContext.Users.Remove(user);
        _appDbContext.SaveChanges();
        return true;
    }
}