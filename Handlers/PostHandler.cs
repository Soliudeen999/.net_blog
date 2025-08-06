using Blog.Services;
using Microsoft.AspNetCore.Mvc;
using Blog.Utils;
using Blog.Models;
using Blog.Utils.Requests;

namespace Blog.Handlers;

public static class PostHandler
{
    public static void AddPostEndpoints(this IEndpointRouteBuilder group)
    {
        group.MapGet("/posts", List).WithName("ListPosts").WithOpenApi();
        group.MapGet("/posts/{slug}", Show).WithName("ShowPost").WithOpenApi();
        group.MapPost("/posts", Create).WithName("CreatePost").WithOpenApi();
        group.MapPut("/posts/{id}", Update).WithName("UpdatePost").WithOpenApi();
        group.MapDelete("/posts/{id}", Delete).WithName("DeletePost").WithOpenApi();
    }

    public static IResult List(
        [FromServices] PostService postService,
        [FromQuery] string? search = null,
        [FromQuery] int? userId = null,
        [FromQuery] int? tagId = null,
        [FromQuery] int page = 1,
        [FromQuery] int perPage = 10
    )
    {
        var posts = postService.GetAll(search: search, userId: userId, tagId: tagId, page: page, perPage:perPage, status : null);
        return ApiResponse.Success(message: "All Posts Retrieved Successfully", data: posts);
    }

    public static IResult Show([FromServices] PostService postService, string slug)
    {
        var post = postService.FindBySlug(slug);

        if (post == null)
        {
            return ApiResponse.Error(message: "Resource not found", statusCode: StatusCodes.Status404NotFound);
        }

        return ApiResponse.Success(message: "Post Retrieved Successfully", data: post);
    }

    public static IResult Create(
        [FromServices] PostService postService,
        [FromServices] AuthService authService,
        HttpContext context,
        CreatePostRequest request)
    {
        var userEmail = context.Session.GetString("user");
        if (string.IsNullOrEmpty(userEmail))
        {
            return ApiResponse.Error(message: "Authentication required", statusCode: StatusCodes.Status401Unauthorized);
        }

        var post = postService.Create(request, userEmail);
        return ApiResponse.Success(message: "Post created successfully", data: post, statusCode: StatusCodes.Status201Created);
    }

    public static IResult Update(
        [FromServices] PostService postService,
        HttpContext context,
        int id,
        UpdatePostRequest request)
    {
        var userEmail = context.Session.GetString("user");
        if (string.IsNullOrEmpty(userEmail))
        {
            return ApiResponse.Error(message: "Authentication required", statusCode: StatusCodes.Status401Unauthorized);
        }

        var result = postService.Update(id, request, userEmail);
        if (result == null)
        {
            return ApiResponse.Error(message: "Post not found or unauthorized", statusCode: StatusCodes.Status404NotFound);
        }

        return ApiResponse.Success(message: "Post updated successfully", data: result);
    }

    public static IResult Delete(
        [FromServices] PostService postService,
        HttpContext context,
        int id)
    {
        var userEmail = context.Session.GetString("user");
        if (string.IsNullOrEmpty(userEmail))
        {
            return ApiResponse.Error(message: "Authentication required", statusCode: StatusCodes.Status401Unauthorized);
        }

        var success = postService.Delete(id, userEmail);
        if (!success)
        {
            return ApiResponse.Error(message: "Post not found or unauthorized", statusCode: StatusCodes.Status404NotFound);
        }

        return ApiResponse.Success(message: "Post deleted successfully");
    }
}
