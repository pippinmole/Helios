using System.ComponentModel.DataAnnotations;
using Helios.Data.Users;
using Helios.Data.Users.Extensions;
using Helios.Extensions;
using Helios.MailService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Pages; 

public class DashboardModel : PageModel {
    private readonly ILogger<DashboardModel> _logger;
    private readonly IAppUserManager _userManager;
    private readonly IMailSender _mailSender;

    public DashboardModel(ILogger<DashboardModel> logger, IAppUserManager userManager, IMailSender mailSender) {
        _logger = logger;
        _userManager = userManager;
        _mailSender = mailSender;
    }

    public async Task<IActionResult> OnGetAsync() {
        var user = await _userManager.GetUserByIdAsync(User.GetUniqueId());
        if ( user == null || !User.IsLoggedIn() ) {
            await _userManager.SignOutAsync();
            return Redirect("/");
        }

        if ( !user.EmailConfirmed ) return Redirect("/verify");

        return Page();
    }

    public async Task<IActionResult> OnPostChangeUsername([MaxLength(15), MinLength(10)] string username) {
        if ( !ModelState.IsValid )
            return Page();
                
        var user = await _userManager.GetUserByIdAsync(User.GetUniqueId());
        if ( user == null ) return Redirect("/");

        user.UserName = username;
            
        await _userManager.UpdateUserAsync(user);
            
        _logger.LogWarning("{OldName} is changing their username to {NewName}", user.UserName, username);

        return Page();
    }
        
    public async Task<IActionResult> OnPostChangePassword() {
        if ( !ModelState.IsValid )
            return Page();
                
        var user = await _userManager.GetUserByIdAsync(User.GetUniqueId());
        if ( user == null ) return Redirect("/");

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        await _mailSender.SendPasswordReset(user.Email, resetToken, this);
            
        _logger.LogInformation("{Username} has requested a password change", user.UserName);
            
        return Page();
    }
}