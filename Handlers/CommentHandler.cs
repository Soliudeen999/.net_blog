using Blog.Services;
using Microsoft.AspNetCore.Mvc;
using Blog.Utils;
using Blog.Utils.Requests;

namespace Blog.Handlers;

public static class CommentHandler
{
    public static void AddCommentEndpoints(this IEndpointRouteBuilder group)
    {
        group.MapGet("/posts/{postId}/comments", List).WithName("ListComments").WithOpenApi();
        group.MapPost("/posts/{postId}/comments", Create).WithName("CreateComment").WithOpenApi();
        group.MapPut("/comments/{id}", Update).WithName("UpdateComment").WithOpenApi();
        group.MapDelete("/comments/{id}", Delete).WithName("DeleteComment").WithOpenApi();
    }

    public static IResult List(
        [FromServices] CommentService commentService,
        int postId,
        [FromQuery] int page = 1,
        [FromQuery] int perPage = 10)
    {
        var comments = commentService.GetByPostId(postId, page, perPage);
        return ApiResponse.Success(message: "Comments retrieved successfully", data: comments);
    }

    public static IResult Create(
        [FromServices] CommentService commentService,
        HttpContext context,
        int postId,
        CreateCommentRequest request)
    {
        var userEmail = context.Session.GetString("user");
        if (string.IsNullOrEmpty(userEmail))
        {
            return ApiResponse.Error(message: "Authentication required", statusCode: StatusCodes.Status401Unauthorized);
        }

        var comment = commentService.Create(postId, request, userEmail);
        if (comment == null)
        {
            return ApiResponse.Error(message: "Post not found", statusCode: StatusCodes.Status404NotFound);
        }

        return ApiResponse.Success(message: "Comment created successfully", data: comment, statusCode: StatusCodes.Status201Created);
    }

    public static IResult Update(
        [FromServices] CommentService commentService,
        HttpContext context,
        int id,
        UpdateCommentRequest request)
    {
        var userEmail = context.Session.GetString("user");
        if (string.IsNullOrEmpty(userEmail))
        {
            return ApiResponse.Error(message: "Authentication required", statusCode: StatusCodes.Status401Unauthorized);
        }

        var result = commentService.Update(id, request, userEmail);
        if (result == null)
        {
            return ApiResponse.Error(message: "Comment not found or Unauthorized", statusCode: StatusCodes.Status404NotFound);
        }

        return ApiResponse.Success(message: "Comment updated successfully", data: result);
    }

    public static IResult Delete(
        [FromServices] CommentService commentService,
        HttpContext context,
        int id)
    {
        var userEmail = context.Session.GetString("user");
        if (string.IsNullOrEmpty(userEmail))
        {
            return ApiResponse.Error(message: "Authentication required", statusCode: StatusCodes.Status401Unauthorized);
        }

        var success = commentService.Delete(id, userEmail);
        if (!success)
        {
            return ApiResponse.Error(message: "Comment not found or Unauthorized", statusCode: StatusCodes.Status404NotFound);
        }

        return ApiResponse.Success(message: "Comment deleted successfully");
    }
} 