namespace Blog.Utils.Middlewares;

public class SessionAuthMiddleware(RequestDelegate _next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower();

        // Skip auth check for public endpoints
        var isPublic =  path == "/api/v1/register" || path == "/api/v1/login" || path == "/api/v1/logout"   ||
                        path == "/api/v1/check-session"    ||
                        path == "/api/v1/swagger"  ||
                        (path != null && path.StartsWith("/scalar"));

        if (!isPublic)
        {
            var user = context.Session.GetString("user");
            if (string.IsNullOrEmpty(user))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { error = "UnUserized" });
                return;
            }
        }

        await _next(context);
    }
}
