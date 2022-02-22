using System.Globalization;
using PayPalCheckoutSdk.Orders;

namespace Helios.Products; 

public static class ProductExtensions {

    public static Item ToPaypalItem(this Product product) {
        return new Item {
            Name = product.Name,
            Description = product.BriefDescription,
            Sku = product.Id.ToString(),
            Category = "DIGITAL_GOODS",
            UnitAmount = new Money() {
                CurrencyCode = "USD",
                Value = product.PriceHnt.ToString(CultureInfo.InvariantCulture)
            },
            Quantity = "1"
        };
    }
    
}