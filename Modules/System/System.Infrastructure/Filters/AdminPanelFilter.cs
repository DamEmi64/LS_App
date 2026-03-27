using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace System.Infrastructure.Filters
{
    public class AdminPanelFilter : IAuthorizationFilter
    {
        private readonly AdminPanelOptions _options;

        public AdminPanelFilter(IOptions<AdminPanelOptions> options)
        {
            _options = options.Value;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var cookie = context.HttpContext.Request.Cookies["adminToken"];

            if (cookie != _options.Token)
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(new { message = "Unauthorized" });
            }
        }
    }

    public class AdminPanelOptions
    {
        public string Token { get; set; } = string.Empty;
    }
}