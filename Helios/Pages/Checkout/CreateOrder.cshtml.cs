using Helios.Data.Users;
using Helios.Data.Users.Extensions;
using Helios.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Pages.Checkout; 

public class CreateOrderModel : PageModel {
    private readonly ILogger<CreateOrderModel> _logger;
    private readonly IAppUserManager _userManager;

    public CreateOrderModel(ILogger<CreateOrderModel> logger, IAppUserManager userManager) {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGetAsync(int id) {
        var user = await _userManager.GetUserByIdAsync(User.GetUniqueId());
        if ( user == null ) return Redirect("/");
        
        // can create a new order
        
        user.Order = new ProductOrder {
            ProductId = id,
            Created = DateTime.Now,
            Memo = user.Id.ToString()[..7] + Product.Tiers[id].Id
        };

        await _userManager.UpdateUserAsync(user);
        
        return Redirect($"/checkout");
    }
}