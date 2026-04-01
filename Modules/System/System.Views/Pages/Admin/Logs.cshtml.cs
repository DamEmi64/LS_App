using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Infrastructure.Filters;

[TypeFilter(typeof(AdminPanelFilter))]
public class LogsModel : PageModel
{
    public void OnGet()
    {
    }
}