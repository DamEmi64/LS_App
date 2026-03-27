using Serilog;

namespace Api
{
    public class ErrorMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
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
}