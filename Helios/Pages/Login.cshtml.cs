using Helios.Data.Users;
using Helios.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Pages; 

public class LoginModel : PageModel {
    private readonly ILogger<LoginModel> _logger;
    private readonly IAppUserManager _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    [BindProperty] public LoginForm LoginForm { get; set; }

    public LoginModel(ILogger<LoginModel> logger, IAppUserManager userManager, SignInManager<ApplicationUser> signInManager) {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }
        
    public async Task<IActionResult> OnPost() {
        if ( !ModelState.IsValid ) {
            foreach ( var error in ViewData.ModelState.Values.SelectMany(modelState => modelState.Errors) ) {
                _logger.LogWarning(error.ErrorMessage);
            }

            return Page();
        }

        _logger.LogInformation("Login attempt for {Username}", LoginForm.Username);

        var result = await _signInManager.PasswordSignInAsync(LoginForm.Username, LoginForm.Password,
            LoginForm.RememberMe, false);

        if ( result.Succeeded ) {
            return RedirectToPage("/Dashboard");
        } else {
            _logger.LogWarning("Login attempt failed");
            ModelState.AddModelError("", "Login failed. Please check the credentials and try again.");
                
            return Page();
        }
    }
}