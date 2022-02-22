using AspNetCoreHero.ToastNotification.Abstractions;
using Helios.Data.Users;
using Helios.Data.Users.Extensions;
using Helios.Helium;
using Helios.MailService;
using Helios.Products;
using Helios.Products.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Helios.Pages;

public class CheckoutModel : PageModel {

    private readonly ILogger<CheckoutModel> _logger;
    private readonly IAppUserManager _userManager;
    private readonly IHeliumService _heliumService;
    private readonly IOptions<HeliumOptions> _heliumOptions;
    private readonly INotyfService _notyfService;
    private readonly IMailSender _mailSender;
    private readonly IOrderValidator _orderValidator;

    public CheckoutModel(ILogger<CheckoutModel> logger, IAppUserManager userManager, IHeliumService heliumService,
        IOptions<HeliumOptions> heliumOptions, INotyfService notyfService, IMailSender mailSender, IOrderValidator orderValidator) {
        _logger = logger;
        _userManager = userManager;
        _heliumService = heliumService;
        _heliumOptions = heliumOptions;
        _notyfService = notyfService;
        _mailSender = mailSender;
        _orderValidator = orderValidator;
    }

    public async Task<IActionResult> OnGetAsync() {
        if ( !User.IsLoggedIn() )
            return Redirect("/");

        var user = await _userManager.GetUserByIdAsync(User.GetUniqueId());
        if ( user?.Order == null ) return Redirect("/");

        return Page();
    }

    public async Task<IActionResult> OnPostTransactionCheckAsync(string hash, CancellationToken cancellationToken) {
        if ( !ModelState.IsValid ) return Page();
        if ( !User.IsLoggedIn() ) return Redirect("/");

        var user = await _userManager.GetUserByIdAsync(User.GetUniqueId());
        if ( user?.Order == null ) return Redirect("/");

        _logger.LogInformation("Checking transaction '{Hash}'", hash);

        var result = await _orderValidator.ValidateOrder(user, user.Order, hash, cancellationToken);
        if ( result.Successful ) {
            // good transaction, upgrade account
            user.AccountType = (EAccountType) user.Order.ProductId;
            user.PreviousOrderHashes.Add(hash);
            user.Order = null;

            await _userManager.UpdateUserAsync(user);   
            
            // Send confirmation email
            await _mailSender.SendPurchaseConfirmedAsync(user.Email, user.Order);

            // TODO: Payment successful page
            return Redirect("/");
        } else {
            ModelState.AddModelError("Error", result.Error);
            
            return Page();
        }
    }
}
