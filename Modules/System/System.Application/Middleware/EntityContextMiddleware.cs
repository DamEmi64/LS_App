using Base;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class EntityContextMiddleware
{
    private readonly RequestDelegate _next;

    public EntityContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IEntityContext userContext)
    {
        var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!string.IsNullOrEmpty(userId))
        {
            userContext.Editor = $"USER({userId})";
        }

        await _next(context);
    }
}