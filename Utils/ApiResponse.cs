namespace Blog.Utils;
public static class ApiResponse
{
    public static IResult Success(object? data = null, string message = "Request Completed", int statusCode = StatusCodes.Status200OK)
    {
        var response = new Dictionary<string, object?>
        {
            ["status"] = true,
            ["message"] = message
        };

        if (data != null)
            response["data"] = data;

        return Results.Json(response, statusCode: statusCode);
    }

    public static IResult Error(object? data = null, string message = "", int statusCode = StatusCodes.Status400BadRequest)
    {
        var response = new Dictionary<string, object?>
        {
            ["status"] = false,
            ["message"] = message
        };

        if (data != null)
            response["data"] = data;

        return Results.Json(response, statusCode: statusCode);
    }
}