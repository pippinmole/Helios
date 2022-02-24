using Helios.Data.Users;
using Helios.Data.Users.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Model_Templates; 

public class AuthorizedPageModel : PageModel {
    private readonly IAppUserManager _userManager;

    public AuthorizedPageModel(IAppUserManager userManager) {
        _userManager = userManager;
    }
    
    public virtual async Task<IActionResult> OnGetAsync() {
        var user = await _userManager.GetUserByIdAsync(User.GetUniqueId());
        if ( user == null || !User.IsLoggedIn() ) {
            await _userManager.SignOutAsync();
            return Redirect("/");
        }

        if ( !user.EmailConfirmed )
            return Redirect("/verify");

        return Page();
    }
    
}