using System.ComponentModel.DataAnnotations;
using Helios.Data.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Pages;

public class ResetPasswordModel : PageModel {
    private readonly ILogger<ResetPasswordModel> _logger;
    private readonly IAppUserManager _userManager;
        
    [BindProperty] public ResetPasswordForm Form { get; set; }

    public ResetPasswordModel(ILogger<ResetPasswordModel> logger, IAppUserManager userManager) {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IActionResult> OnPostAsync(string email) {
        if ( !ModelState.IsValid )
            return Page();

        var user = await _userManager.GetUserByEmailAsync(email);
        if ( user == null ) return Forbid();

        var token = HttpContext.Request.Query["token"];
        
        _logger.LogInformation(token);
        
        var result = await _userManager.ResetPasswordAsync(user, token, Form.Password);
        if ( result.Succeeded ) return LocalRedirect("/Login");
        
        foreach ( var error in result.Errors ) {
            ModelState.TryAddModelError(error.Code, error.Description);
        }

        return Page();

    }
        
    public class ResetPasswordForm {
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}