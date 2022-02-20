using AspNetCoreHero.ToastNotification.Abstractions;
using Helios.Data.Users;
using Helios.Data.Users.Extensions;
using Helios.Forms;
using Microsoft.AspNetCore.Mvc;

namespace Helios.Pages.Dashboard;

public class AccountModel : DashboardModel {
    private readonly ILogger<AccountModel> _logger;
    private readonly IAppUserManager _userManager;
    private readonly INotyfService _notyfService;

    [BindProperty] public ChangeAccountForm Form { get; set; }

    public AccountModel(ILogger<AccountModel> logger, IAppUserManager userManager, INotyfService notyfService) :
        base(userManager) {
        _logger = logger;
        _userManager = userManager;
        _notyfService = notyfService;
    }

    public async Task<IActionResult> OnPostAsync() {
        if ( !ModelState.IsValid )
            return Page();

        var user = await _userManager.GetUserByIdAsync(User.GetUniqueId());
        if ( user == null ) return Redirect("/");

        user.UserName = Form.Username;

        if ( user.Email != Form.Email ) {
            // Changed email, will need to verify again
            user.Email = Form.Email;
            user.EmailConfirmed = false;
        }

        user.DowntimeNotifyRate = Form.NewNotifyTimespan;
        user.ReceiveEmails = Form.ReceiveEmails;

        await _userManager.UpdateUserAsync(user);
        await _userManager.ResetPasswordAsync(user, Form.CurrentPassword, Form.NewPassword);

        _notyfService.Success("Successfully updated profile");

        return Redirect("/dashboard/profile");
    }
}