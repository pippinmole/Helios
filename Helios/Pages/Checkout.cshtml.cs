using Helios.Data.Users.Extensions;
using Helios.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Pages; 

public class CheckoutModel : PageModel {
    
    private readonly ILogger<CheckoutModel> _logger;

    public List<Product> Cart { get; set; }

    public CheckoutModel(ILogger<CheckoutModel> logger) {
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(int id) {
        if ( !User.IsLoggedIn() )
            return Redirect("/");
        
        Cart = new List<Product> {Product.Tiers[id]};

        return Page();
    }
}