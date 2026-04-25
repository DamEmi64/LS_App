using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Text;

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
            var request = context.HttpContext.Request;

            if (!request.Headers.ContainsKey("Authorization"))
            {
                Challenge(context);
                return;
            }

            var authHeader = request.Headers["Authorization"].ToString();

            if (!authHeader.StartsWith("Basic "))
            {
                Challenge(context);
                return;
            }

            var encoded = authHeader.Substring("Basic ".Length).Trim();
            var credentialBytes = Convert.FromBase64String(encoded);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');

            if (credentials.Length != 2)
            {
                Challenge(context);
                return;
            }

            var username = credentials[0];
            var password = credentials[1];

            if (username != _options.Login || password != _options.Password)
            {
                Challenge(context);
                return;
            }
        }

        private void Challenge(AuthorizationFilterContext context)
        {
            context.HttpContext.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Admin Panel\"";
            context.Result = new UnauthorizedResult();
        }
    }

    public class AdminPanelOptions
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}