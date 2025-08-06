using Blog.Models;
using Blog.Configs;
using Blog.Services;

namespace Blog.App;

public static class Container
{
    public static void RegisterCustomServices(this WebApplicationBuilder service)
    {
        service.Services.AddEndpointsApiExplorer();
        service.Services.AddSwaggerGen();

        // Creating CORS config
        service.Services.AddCorsConfig();

        // Autoloading Injected Classes
        service.Services.AddScoped<Comment>();
        service.Services.AddScoped<Post>();
        service.Services.AddScoped<PostTag>();
        service.Services.AddScoped<Reaction>();
        service.Services.AddScoped<Tag>();
        service.Services.AddScoped<User>();

        service.Services.AddTransient<AuthService>();
        service.Services.AddScoped<PostService>();
        service.Services.AddScoped<UserService>();
        service.Services.AddScoped<CommentService>();
        service.Services.AddScoped<TagService>();
        service.Services.AddScoped<ReactionService>();
    }
}