using Blog.Services;
using Microsoft.AspNetCore.Mvc;
using Blog.Utils;
using Blog.Utils.Requests;

namespace Blog.Handlers;

public static class TagHandler
{
    public static void AddTagEndpoints(this IEndpointRouteBuilder group)
    {
        group.MapGet("/tags", List).WithName("ListTags").WithOpenApi();
        group.MapPost("/tags", Create).WithName("CreateTag").WithOpenApi();
        group.MapDelete("/tags/{id}", Delete).WithName("DeleteTag").WithOpenApi();
    }

    public static IResult List([FromServices] TagService tagService)
    {
        var tags = tagService.GetAll();
        return ApiResponse.Success(message: "Tags retrieved successfully", data: tags);
    }

    public static IResult Create(
        [FromServices] TagService tagService,
        CreateTagRequest request)
    {
        var tag = tagService.Create(request.Name);
        if (tag == null)
        {
            return ApiResponse.Error(message: "Tag with this name already exists", statusCode: StatusCodes.Status400BadRequest);
        }

        return ApiResponse.Success(message: "Tag created successfully", data: tag, statusCode: StatusCodes.Status201Created);
    }

    public static IResult Delete([FromServices] TagService tagService, int id)
    {
        var success = tagService.Delete(id);
        if (!success)
        {
            return ApiResponse.Error(message: "Tag not found", statusCode: StatusCodes.Status404NotFound);
        }

        return ApiResponse.Success(message: "Tag deleted successfully");
    }
} 