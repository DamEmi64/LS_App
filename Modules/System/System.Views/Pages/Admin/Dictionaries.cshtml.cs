using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Infrastructure.Services.Admin;

public class DictionariesModel : PageModel
{
    private readonly IAdminService _adminService;

    public DictionariesModel(IAdminService adminService)
    {
        _adminService = adminService;
    }

    public List<string> DictionaryNames { get; set; } = new();

    public async Task OnGetAsync()
    {
        var data = _adminService.GetDictionaries();
        DictionaryNames = data
            .Select(x => x.Dictionary)
            .Distinct()
            .OrderBy(x => x)
            .ToList();
    }

    public async Task<IActionResult> OnGetDataAsync(string? dictionary)
    {
        var data = _adminService.GetDictionaries();

        if (!string.IsNullOrEmpty(dictionary))
        {
            data = data.Where(x => x.Dictionary == dictionary).ToList();
        }

        return new JsonResult(data);
    }
}