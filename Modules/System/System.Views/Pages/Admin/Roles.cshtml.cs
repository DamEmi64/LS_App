using Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Infrastructure.Filters;

namespace System.Views.Pages.Admin
{
    [TypeFilter(typeof(AdminPanelFilter))]
    public class RolesModel : PageModel
    {
        private readonly IConnector _connector;

        public RolesModel(IConnector connector)
        {
            _connector = connector;
        }

        public Dictionary<string, string>? Claims { get; set; }

        public void OnGet()
        {
            Claims = _connector.Permissions.ToDictionary(PermissionInfo => PermissionInfo.Key, PermissionInfo => PermissionInfo.Description);
        }
    }
}
