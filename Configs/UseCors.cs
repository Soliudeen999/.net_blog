namespace Blog.Configs;

public static class UseCors
{
    public static void AddCorsConfig(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });
        });
    }

    public static void ApplyCors(this WebApplication application)
    {
        application.UseCors("AllowAll");
    }
}