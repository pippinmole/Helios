using Helios.Data.Users;
using Helios.Data.Users.Extensions;
using Helios.MailService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Pages;

public class IndexModel : PageModel {
    private readonly ILogger<IndexModel> _logger;
    private readonly IAppUserManager _userManager;
    private readonly IMailSender _mailSender;

    public IndexModel(ILogger<IndexModel> logger, IAppUserManager userManager, IMailSender mailSender) {
        _logger = logger;
        _userManager = userManager;
        _mailSender = mailSender;
    }

    public async Task<IActionResult> OnGetAsync() {
        if ( !User.IsLoggedIn() ) return Page();

        _logger.LogInformation("Sending email");
        await _mailSender.SendServiceDownAsync("icondesk1@gmail.com", CancellationToken.None);
        
        var user = await _userManager.GetUserByIdAsync(User.GetUniqueId());
        if ( user == null ) return Page();

        return user.EmailConfirmed
            ? Redirect("dashboard")
            : Redirect("verify");
    }
}