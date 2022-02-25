using System.ComponentModel.DataAnnotations;
using AspNetCoreHero.ToastNotification.Abstractions;
using Helios.Data.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Pages;

public class ResetPasswordModel : PageModel {
    private readonly ILogger<ResetPasswordModel> _logger;
    private readonly IAppUserManager _userManager;
    private readonly INotyfService _notyfService;

    [BindProperty] public ResetPasswordForm Form { get; set; }

    public ResetPasswordModel(ILogger<ResetPasswordModel> logger, IAppUserManager userManager, INotyfService notyfService) {
        _logger = logger;
        _userManager = userManager;
        _notyfService = notyfService;
    }

    public async Task<IActionResult> OnPostAsync(string email) {
        if ( !ModelState.IsValid )
            return Page();

        var user = await _userManager.GetUserByEmailAsync(email);
        if ( user == null ) return Forbid();

        var token = HttpContext.Request.Query["token"];
        var result = await _userManager.ResetPasswordAsync(user, token, Form.Password);
        if ( result.Succeeded ) {
            _notyfService.Success("Successfully reset password");
            return LocalRedirect("/Login");
        }

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