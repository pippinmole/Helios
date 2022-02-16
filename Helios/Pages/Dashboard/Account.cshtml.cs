using System.ComponentModel.DataAnnotations;
using Helios.Data.Users;
using Helios.Data.Users.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Pages.Dashboard; 

public class AccountModel : DashboardModel {
    private readonly ILogger<AccountModel> _logger;
    private readonly IAppUserManager _userManager;
    
    public AccountModel(ILogger<AccountModel> logger, IAppUserManager userManager) : base(userManager) {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IActionResult> OnPostUpdateAccountType(EAccountType accountType) {
        _logger.LogInformation("AccountTypeId: {Id}", accountType);
        
        if ( !ModelState.IsValid )
            return Page();
                
        var user = await _userManager.GetUserByIdAsync(User.GetUniqueId());
        if ( user == null ) return Redirect("/");

        user.AccountType = accountType;

        _logger.LogInformation("Set user account type to {Type}", (EAccountType) accountType);
        
        await _userManager.UpdateUserAsync(user);

        return Page();
    }

    // public async Task<IActionResult> OnPostChangeUsername([MaxLength(15), MinLength(10)] string username) {
    //     if ( !ModelState.IsValid )
    //         return Page();
    //             
    //     var user = await _userManager.GetUserByIdAsync(User.GetUniqueId());
    //     if ( user == null ) return Redirect("/");
    //
    //     user.UserName = username;
    //         
    //     await _userManager.UpdateUserAsync(user);
    //         
    //     _logger.LogWarning("{OldName} is changing their username to {NewName}", user.UserName, username);
    //
    //     return Page();
    // }
    //     
    // public async Task<IActionResult> OnPostChangePassword() {
    //     if ( !ModelState.IsValid )
    //         return Page();
    //             
    //     var user = await _userManager.GetUserByIdAsync(User.GetUniqueId());
    //     if ( user == null ) return Redirect("/");
    //
    //     var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
    //     await _mailSender.SendPasswordReset(user.Email, resetToken, this);
    //         
    //     _logger.LogInformation("{Username} has requested a password change", user.UserName);
    //         
    //     return Page();
    // }
}