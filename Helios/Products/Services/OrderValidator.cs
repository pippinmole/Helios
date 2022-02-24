using Helios.Data.Users;
using Helios.Helium;
using Helios.Helium.Helpers;
using Microsoft.Extensions.Options;

namespace Helios.Products.Services;

public class OrderValidateResult {
    public bool Successful => Error == null;
    public string Error { get; set; }
    public Product Product { get; set; }
}

public class OrderValidator : IOrderValidator {
    private readonly ILogger<OrderValidator> _logger;
    private readonly IOptions<HeliumOptions> _heliumOptions;
    private readonly IHeliumService _heliumService;

    public OrderValidator(ILogger<OrderValidator> logger, IOptions<HeliumOptions> heliumOptions, IHeliumService heliumService) {
        _logger = logger;
        _heliumOptions = heliumOptions;
        _heliumService = heliumService;
    }

    public async Task<OrderValidateResult> ValidateOrder(ApplicationUser user, string hash, CancellationToken cancellationToken = default) {

        var result = new OrderValidateResult();

        if ( !HeliumHelper.IsValidHash(hash) ) {
            result.Error = "Transaction hash is not in the right format";
            return result;
        }
        
        var (block, payment) = await _heliumService.GetTransactionFromHash(hash, cancellationToken);

        if ( block == null || payment == null ) {
            result.Error = "No transaction was found with that hash";
            return result;
        }

        var memo = payment.GetDecodedMemo();
        if ( !int.TryParse(memo[7].ToString(), out var productId) ) {
            result.Error = "Memo is malformed";
            return result;
        }
        
        if ( memo[..7] != user.Id.ToString()[..7] ) {
            result.Error = "Memo does not match user";
            return result;
        }

        // check against users previous transactions - have they already redeemed this?
        if ( user.PreviousOrderHashes.Any(x => x == hash) ) {
            result.Error = "You have already redeemed this transaction";
            return result;
        }

        if ( payment.payee != _heliumOptions.Value.TransactionAddress ) {
            // Not paid to our address, invalid.
            result.Error = "Transaction invalid";
            return result;
        }

        var product = Product.Tiers[productId];
        if ( payment.AmountHnt < product.PriceHnt ) {
            result.Error = "Not enough HNT paid";
            return result;
        }

        result.Product = product;
        return result;
    }
}

public interface IOrderValidator {
    Task<OrderValidateResult> ValidateOrder(ApplicationUser user, string hash,
        CancellationToken cancellationToken = default);
}