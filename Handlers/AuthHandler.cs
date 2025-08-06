using Blog.Services;
using Blog.Utils.Requests;
using Microsoft.AspNetCore.Mvc;
using Blog.Utils;


namespace Blog.Handlers;

public static class AuthHandler
{
    public static void AddAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/login", Login).WithName("Login").WithOpenApi();
        app.MapGet("/check-session", CheckSession).WithName("CheckSession").WithOpenApi();
        app.MapPost("/logout", Logout).WithName("Logout").WithOpenApi();
    }

    public static IResult Login(HttpContext context, LoginRequest loginRequest, [FromServices] AuthService authService)
    {
        if (authService.Login(loginRequest))
        {
            context.Session.SetString("user", loginRequest.Email);
            return ApiResponse.Success(message: "Login successful");
        }

        return ApiResponse.Error(message: "Invalid credentials", statusCode: StatusCodes.Status401Unauthorized);
    }

    public static IResult CheckSession(HttpContext context)
    {
        var user = context.Session.GetString("user");
        if (string.IsNullOrEmpty(user))
            return ApiResponse.Error(message: "Not logged in", statusCode: StatusCodes.Status401Unauthorized);

        return ApiResponse.Success(new { user });
    }

    public static IResult Logout(HttpContext context)
    {
        context.Session.Clear();
        return ApiResponse.Success("Logged out successfully");
    }
}
