using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Pages.Dashboard; 

public class DashboardIndexModel : PageModel {
    public IActionResult OnGet() => RedirectToPage("Devices");
}