using Blog.Configs;
using Blog.Models;
namespace Blog;

public static class ServiceRegistry
{
    public static void RegisterServices(this WebApplicationBuilder service)
    {
        service.Services.AddEndpointsApiExplorer();
        service.Services.AddSwaggerGen();

        service.Services.AddDistributedMemoryCache();

        service.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); // session timeout
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        // Creating CORS config
        service.Services.AddCorsConfig();

        // Autoloading Injected Classes
        service.Services.AddTransient<Post>();

    }
}