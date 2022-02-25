using AspNetCoreHero.ToastNotification.Abstractions;
using Helios.Data.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Pages; 

public class SignupVerification : PageModel {
    private readonly ILogger<SignupVerification> _logger;
    private readonly IAppUserManager _userManager;
    private readonly INotyfService _notyfService;

    public SignupVerification(ILogger<SignupVerification> logger, IAppUserManager userManager, INotyfService notyfService) {
        _logger = logger;
        _userManager = userManager;
        _notyfService = notyfService;
    }
        
    public async Task<IActionResult> OnGetAsync(string email, string token) {
        if ( !ModelState.IsValid ) return Page();

        var user = await _userManager.GetUserByEmailAsync(email);
        if ( user == null ) return Redirect("/");

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if ( result.Succeeded ) {
            await _userManager.SignInAsync(user, true);

            _notyfService.Success("Email successfully verified!");
        } else {
            foreach ( var error in result.Errors ) {
                ModelState.AddModelError(error.Code, error.Description);
            }
            
            _notyfService.Warning("Failed to verify email");
        }

        return Redirect("/");
    }
}