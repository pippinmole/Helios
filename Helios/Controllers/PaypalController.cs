using System.Globalization;
using System.Net;
using Helios.Data.Users;
using Helios.Data.Users.Extensions;
using Helios.Paypal;
using Helios.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Newtonsoft.Json;
using PayPalCheckoutSdk.Orders;
using PayPalCheckoutSdk.Core;
using HttpResponse = PayPalHttp.HttpResponse;

namespace Helios.Controllers;

[Route("checkout/api/paypal/order")]
public class PaypalController : ControllerBase {
    private readonly ILogger<PaypalController> _logger;
    private readonly IAppUserManager _userManager;
    private readonly IPaypalDatabase _paypalDatabase;
    private readonly PayPalHttp.HttpClient _paypalClient;

    public PaypalController(ILogger<PaypalController> logger, IAppUserManager userManager,
        IOptions<PaypalOptions> paypalOptions, IPaypalDatabase paypalDatabase) {
        _logger = logger;
        _userManager = userManager;
        _paypalDatabase = paypalDatabase;

        _paypalClient =
            new PayPalHttpClient(new SandboxEnvironment(paypalOptions.Value.ClientId,
                paypalOptions.Value.ClientSecret));
    }

    [HttpPost("create/{sku:int}")]
    public async Task<JsonResult> OnOrderCreate(int sku) {
        
        _logger.LogInformation("Starting order for sku {Sku}", sku);

        var product = Product.Tiers[sku];
        if ( product == null ) return new JsonResult("");
        
        // Construct a request object and set desired parameters
        // Here, OrdersCreateRequest() creates a POST request to /v2/checkout/orders
        var order = new OrderRequest {
            CheckoutPaymentIntent = "CAPTURE",
            PurchaseUnits = new List<PurchaseUnitRequest> {
                new() {
                    AmountWithBreakdown = new AmountWithBreakdown {
                        CurrencyCode = "USD",
                        Value = product.PriceUsd.ToString(CultureInfo.InvariantCulture),
                        AmountBreakdown = new AmountBreakdown() {
                            Discount = new Money() { CurrencyCode = "USD", Value = "0"},
                            Handling = new Money() { CurrencyCode = "USD", Value = "0"},
                            Insurance = new Money() { CurrencyCode = "USD", Value = "0"},
                            ItemTotal = new Money() { CurrencyCode = "USD", Value = product.PriceUsd.ToString(CultureInfo.InvariantCulture)},
                            Shipping = new Money() { CurrencyCode = "USD", Value = "0"},
                            ShippingDiscount = new Money() { CurrencyCode = "USD", Value = "0"},
                            TaxTotal = new Money() { CurrencyCode = "USD", Value = "0"},
                        }
                    }
                }
            },
            ApplicationContext = new ApplicationContext {
                ReturnUrl =
                    $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/checkout/api/paypal/order/capture",
                CancelUrl = "https://www.example.com"
            }
        };

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        
        // Call API with your client and get a response for your call
        var request = new OrdersCreateRequest();
        request.Prefer("return=representation");
        request.RequestBody(order);
        
        var response = await _paypalClient.Execute(request);
        var result = response.Result<Order>();

        _logger.LogInformation("Status: {Statue}" +
                               "\n \t Order Id: {OrderId}" +
                               "\n \t Intent: {Intent}" +
                               "\n \t Links: ", result.Status, result.Id, result.CheckoutPaymentIntent);

        foreach ( var link in result.Links ) {
            _logger.LogInformation("\t{Rel}: {Href}\tCall Type: {Method}", link.Rel, link.Href, link.Method);
        }

        var paypalOrder = new PaypalOrder(sku, result.Id);
        await _paypalDatabase.AddOrder(paypalOrder);
        
        return new JsonResult(result);
    }

    [HttpPost("capture/{id}")]
    public async Task<Order> OnOrderCapture(string id) {
        _logger.LogInformation("Capturing order id: {Id}", id);

        // Construct a request object and set desired parameters
        var request = new OrdersCaptureRequest(id);
        request.RequestBody(new OrderActionRequest());

        var response = await _paypalClient.Execute(request);
        var result = response.Result<Order>();
        if ( response.StatusCode != HttpStatusCode.Created ) {
            _logger.LogCritical("Error when trying to capture payment for id {Id}. Status Code: {Code}", id,
                response.StatusCode);
            return null;
        }

        var user = await _userManager.GetUserByIdAsync(User.GetUniqueId());
        if ( user == null ) {
            _logger.LogCritical(
                "Critical error trying to get user information after capturing and processing PayPal payment");
            return null;
        }

        // elevate user here
        // TODO: Allow multiple items in the product list

        var paypalOrder = await _paypalDatabase.GetOrderById(result.Id);
        user.AccountType = (EAccountType) paypalOrder.Sku;

        await _userManager.UpdateUserAsync(user);
        
        _logger.LogInformation("Elevated {UserName} to {AccountType}", user.UserName, (EAccountType) paypalOrder.Sku);

        return result;
    }
}