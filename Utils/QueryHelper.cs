using System.Linq.Expressions;
using Blog.Models;
using System.Linq;               // Required for IQueryable extensions

namespace Blog.Utils;

public static class QueryHelper
{
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int page, int perPage)
    {
        return query.Skip((page - 1) * perPage).Take(perPage);
    }

    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string? orderBy, bool descending = true)
    {
        if (string.IsNullOrWhiteSpace(orderBy)) return query;

        var param = Expression.Parameter(typeof(T), "x");
        var prop = Expression.Property(param, orderBy);
        var lambda = Expression.Lambda(prop, param);

        string method = descending ? "OrderByDescending" : "OrderBy";

        var result = Expression.Call(typeof(Queryable), method,
            new[] { typeof(T), prop.Type },
            query.Expression, Expression.Quote(lambda));

        return query.Provider.CreateQuery<T>(result);
    }

    public static IQueryable<Post> ApplyPostFilters(
        this IQueryable<Post> query,
        string? search,
        int? userId,
        int? tagId,
        Status? status = null)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p =>
                (p.Title != null && p.Title.Contains(search)) ||
                (p.Content != null && p.Content.Contains(search)));
        }

        if (userId.HasValue)
            query = query.Where(p => p.UserId == userId.Value);

        if (tagId.HasValue)
            query = query.Where(p => p.PostTags.Any(pt => pt.TagId == tagId.Value));

        if (status.HasValue)
            query = query.Where(p => p.Status == status.Value);

        return query;
    }
}
