using System.Web;
using Helios.Data.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Pages; 

public class SignupVerification : PageModel {
    private readonly ILogger<SignupVerification> _logger;
    private readonly IAppUserManager _userManager;

    public SignupVerification(ILogger<SignupVerification> logger, IAppUserManager userManager) {
        _logger = logger;
        _userManager = userManager;
    }
        
    public async Task<IActionResult> OnGetAsync(string token, string email) {

        if ( !ModelState.IsValid ) {
            _logger.LogWarning("Model state is not valid");
            return Page();
        }

        var user = await _userManager.GetUserByEmailAsync(email);
        if ( user == null ) return Redirect("/");

        var result = await _userManager.ConfirmEmailAsync(user, HttpUtility.UrlDecode(token));

        if ( !result.Succeeded ) {
            foreach ( var error in result.Errors ) {
                _logger.LogWarning(error.Code + " : " + error.Description);
                ModelState.AddModelError(error.Code, error.Description);
            }

            return Page();
        }

        await _userManager.SignInAsync(user, true);

        return Page();
    }
}