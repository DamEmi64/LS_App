using Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Infrastructure.Filters;

namespace System.Views.Pages.Admin
{
    [TypeFilter(typeof(AdminPanelFilter))]
    public class RolesModel : PageModel
    {
        public Dictionary<string, string>? Claims { get; set; }

        public void OnGet()
        {
            Claims = AppConfiguration.Permissions.ToDictionary(PermissionInfo => PermissionInfo.Key, PermissionInfo => PermissionInfo.Description);
        }
    }
}
