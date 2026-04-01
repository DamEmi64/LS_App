using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Infrastructure.Filters;
using System.Infrastructure.Services.Admin;

namespace System.Views.Pages.Admin
{
    [TypeFilter(typeof(AdminPanelFilter))]
    public class UsersModel : PageModel
    {
        private readonly IAdminService _adminService;

        public UsersModel(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public IEnumerable<IdentityRole> Roles { get; set; } = Array.Empty<IdentityRole>();

        public void OnGet()
        {
            Roles = _adminService.GetRoles();
        }
    }
}
