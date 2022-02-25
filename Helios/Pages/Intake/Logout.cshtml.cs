using Helios.Data.Users;
using Helios.Data.Users.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Pages; 

public class LogoutModel : PageModel {
    private readonly ILogger<LogoutModel> _logger;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LogoutModel(ILogger<LogoutModel> logger, SignInManager<ApplicationUser> signInManager) {
        _logger = logger;
        _signInManager = signInManager;
    }
        
    public async Task<IActionResult> OnGetAsync() {
        _logger.LogInformation("Sign out request from {Username}", User.GetDisplayName());
        await _signInManager.SignOutAsync();

        return RedirectToPage("/Index");
    }
}