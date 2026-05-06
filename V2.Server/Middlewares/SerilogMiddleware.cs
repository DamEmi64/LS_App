using Serilog.Context;

namespace Api
{
    public class SerilogMiddleware
    {
        private readonly RequestDelegate _next;

        public SerilogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            using (LogContext.PushProperty("HttpMethod", context.Request.Method))
            using (LogContext.PushProperty("HttpUri", context.Request.Path.ToString()))
            using (LogContext.PushProperty("User", context.User?.Identity?.Name ?? "Anonymous"))
            {
                await _next(context);
            }
        }
    }
}
