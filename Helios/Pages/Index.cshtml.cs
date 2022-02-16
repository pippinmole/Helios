using Helios.Data.Users;
using Helios.Data.Users.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Pages;

public class IndexModel : PageModel {
    private readonly ILogger<IndexModel> _logger;
    private readonly IAppUserManager _userManager;

    public IndexModel(ILogger<IndexModel> logger, IAppUserManager userManager) {
        _logger = logger;
        _userManager = userManager;
    }
    
    public async Task<IActionResult> OnGetAsync() {
        if ( !User.IsLoggedIn() ) return Page();
        
        var user = await _userManager.GetUserByIdAsync(User.GetUniqueId());
        if ( user == null ) return Page();
        
        return user.EmailConfirmed
            ? Redirect("dashboard")
            : Redirect("verify");
    }
}