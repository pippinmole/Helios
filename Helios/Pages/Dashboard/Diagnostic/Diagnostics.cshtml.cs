using Helios.Data.Users;
using Helios.Data.Users.Extensions;
using Helios.Helium;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Pages.Dashboard.Diagnostic; 

public class DiagnosticsModel : PageModel {
    private readonly ILogger<DiagnosticsModel> _logger;
    private readonly IAppUserManager _userManager;
    
    [BindProperty] public HeliumMiner Device { get; set; }

    public DiagnosticsModel(ILogger<DiagnosticsModel> logger, IAppUserManager userManager) {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGetAsync(string id) {
        var user = await _userManager.GetUserByIdAsync(User.GetUniqueId());
        if ( user == null ) return Redirect("/");
        
        Device = user.Devices.FirstOrDefault(x => x.Id.ToString() == id);

        if ( Device == null )
            return Redirect("/dashboard");

        return Page();
    }
}