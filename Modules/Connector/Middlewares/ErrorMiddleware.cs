using Microsoft.AspNetCore.Http;
using Serilog;

public class ErrorMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            Log.Error(ex, "An unhandled exception has occurred while executing the request.");

            var errorResponse = new
            {
                Message = ex.Message,
                InnerMessage = ex.InnerException?.Message,
            };
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}