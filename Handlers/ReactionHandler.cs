using Blog.Services;
using Microsoft.AspNetCore.Mvc;
using Blog.Utils;
using Blog.Utils.Requests;

namespace Blog.Handlers;

public static class ReactionHandler
{
    public static void AddReactionEndpoints(this IEndpointRouteBuilder group)
    {
        group.MapGet("/posts/{postId}/reactions", List).WithName("ListReactions").WithOpenApi();
        group.MapPost("/posts/{postId}/reactions", Create).WithName("CreateReaction").WithOpenApi();
        group.MapDelete("/posts/{postId}/reactions/{reactionId}", Delete).WithName("DeleteReaction").WithOpenApi();
        group.MapGet("/posts/{postId}/reactions/summary", Summary).WithName("ReactionSummary").WithOpenApi();
    }

    public static IResult List(
        [FromServices] ReactionService reactionService,
        int postId)
    {
        var reactions = reactionService.GetByPostId(postId);
        return ApiResponse.Success(message: "Reactions retrieved successfully", data: reactions);
    }

    public static IResult Create(
        [FromServices] ReactionService reactionService,
        HttpContext context,
        int postId,
        CreateReactionRequest request)
    {
        var userEmail = context.Session.GetString("user");
        if (string.IsNullOrEmpty(userEmail))
        {
            return ApiResponse.Error(message: "Authentication required", statusCode: StatusCodes.Status401Unauthorized);
        }

        var reaction = reactionService.Create(postId, request, userEmail);
        if (reaction == null)
        {
            return ApiResponse.Error(message: "Post not found or reaction already exists", statusCode: StatusCodes.Status400BadRequest);
        }

        return ApiResponse.Success(message: "Reaction created successfully", data: reaction, statusCode: StatusCodes.Status201Created);
    }

    public static IResult Delete(
        [FromServices] ReactionService reactionService,
        HttpContext context,
        int postId,
        int reactionId)
    {
        var userEmail = context.Session.GetString("user");
        if (string.IsNullOrEmpty(userEmail))
        {
            return ApiResponse.Error(message: "Authentication required", statusCode: StatusCodes.Status401Unauthorized);
        }

        var success = reactionService.Delete(reactionId, userEmail);
        if (!success)
        {
            return ApiResponse.Error(message: "Reaction not found or unauthorized", statusCode: StatusCodes.Status404NotFound);
        }

        return ApiResponse.Success(message: "Reaction deleted successfully");
    }

    public static IResult Summary(
        [FromServices] ReactionService reactionService,
        int postId)
    {
        var summary = reactionService.GetReactionSummary(postId);
        return ApiResponse.Success(message: "Reaction summary retrieved successfully", data: summary);
    }
} 