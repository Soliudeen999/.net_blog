using Blog.Handlers;

namespace Blog.Routes;

public static class AppRoutes
{
    public static void AddRoutes(this WebApplication app)
    {
        var group = app.MapGroup("api/v1");

        group.AddAuthEndpoints();
        group.AddPostEndpoints();
        group.AddCommentEndpoints();
        group.AddTagEndpoints();
        group.AddReactionEndpoints();
    }
}