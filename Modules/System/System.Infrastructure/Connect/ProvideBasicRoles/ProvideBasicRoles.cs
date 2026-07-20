using Base;
using System.Infrastructure.Services.Admin;

namespace System.Infrastructure.Connect.ProvideBasicRoles
{
    public class ProvideBasicRoles : ConnectInstance<Base.ProvideBasicRoles>
    {
        private readonly IAdminService _adminService;

        public ProvideBasicRoles(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public override async Task HandleAsync(Base.ProvideBasicRoles request)
        {
            var roles = _adminService.GetRoles();
            if (!roles.Select(x => x.Name).Contains("admin"))
            {
                await _adminService.CreateRole("admin", request.Permissions.Select(x => x.Key));
            }
            else
            {
                await _adminService.UpdateRole("admin", request.Permissions.Select(x => x.Key));
            }

            if (!roles.Select(x => x.Name).Contains("user"))
            {
                await _adminService.CreateRole("user", request.Permissions.Where(x => x.IsBasic).Select(x => x.Key));
            }
            else
            {
                await _adminService.UpdateRole("user", request.Permissions.Where(x => x.IsBasic).Select(x => x.Key));
            }
        }
    }
}