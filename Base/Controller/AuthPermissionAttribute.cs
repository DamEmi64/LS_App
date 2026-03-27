using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Base
{
    /// <summary>
    ///     Authorization attribute
    /// </summary>
    public class AuthPermissionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _permissions;

        public AuthPermissionAttribute(params string[] permissions)
        {
            _permissions = permissions;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (user.IsInRole("admin"))
            {
                return;
            }

            if (_permissions.Length > 0)
            {
                var userPermissions = user.Claims
                    .Where(c => c.Type == "permission")
                    .Select(c => c.Value);
                if (!_permissions.Any(p => userPermissions.Contains(p)))
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
        }
    }
}