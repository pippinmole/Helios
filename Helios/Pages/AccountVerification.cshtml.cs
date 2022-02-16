using Helios.Data.Users;
using Helios.Data.Users.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Pages; 

public class AccountVerificationModel : PageModel {
    private readonly ILogger<AccountVerificationModel> _logger;
    private readonly IAppUserManager _userManager;

    public AccountVerificationModel(ILogger<AccountVerificationModel> logger, IAppUserManager userManager) {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGetAsync() {
        var user = await _userManager.GetUserByIdAsync(User.GetUniqueId());
        if ( user == null || user.EmailConfirmed )
            return Redirect("/");

        return Page();
    }

    public async Task<JsonResult> OnGetIsEmailVerified() {
        var user = await _userManager.GetUserByIdAsync(User.GetUniqueId());
        if ( user == null ) {
            return new JsonResult("");
        }
            
        return new JsonResult(new {
            isVerified = user.EmailConfirmed
        });
    }
}