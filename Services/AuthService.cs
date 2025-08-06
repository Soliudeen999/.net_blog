using Blog.Models;
using Blog.Utils.Requests;
using Microsoft.AspNetCore.Identity;

namespace Blog.Services;

public interface IAuthService
{
    bool Login(LoginRequest loginRequest);
    string HashPassword(User user, string password);
}

public class AuthService : IAuthService
{
    private readonly AppDbContext _dbContext;

    public AuthService(AppDbContext context)
    {
        _dbContext = context;
    }

    private IQueryable<User> UserQuery => _dbContext.Users;
    private readonly PasswordHasher<User> _hasher = new();

    public bool Login(LoginRequest loginRequest)
    {
        var user = UserQuery.FirstOrDefault(u => u.Email == loginRequest.Email);
        if (user is null) return false;

        var result = _hasher.VerifyHashedPassword(user, user.Password ?? "", loginRequest.Password);
        return result == PasswordVerificationResult.Success;
    }

    public string HashPassword(User user, string password)
    {
        return _hasher.HashPassword(user, password);
    }
}
