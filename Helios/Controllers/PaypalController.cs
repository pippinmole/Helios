using System.Net;
using System.Runtime.Serialization;
using Helios.Data.Users;
using Helios.Data.Users.Extensions;
using Helios.Paypal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PayPalCheckoutSdk.Orders;
using PayPalCheckoutSdk.Core;
using HttpResponse = PayPalHttp.HttpResponse;

namespace Helios.Controllers;

[Route("checkout/api/paypal/order")]
public class PaypalController : ControllerBase {
    private readonly ILogger<PaypalController> _logger;
    private readonly IAppUserManager _userManager;
    private readonly PayPalHttp.HttpClient _paypalClient;

    public PaypalController(ILogger<PaypalController> logger, IAppUserManager userManager,
        IOptions<PaypalOptions> paypalOptions) {
        _logger = logger;
        _userManager = userManager;

        _paypalClient =
            new PayPalHttpClient(new SandboxEnvironment(paypalOptions.Value.ClientId,
                paypalOptions.Value.ClientSecret));
    }

    [HttpPost("create")]
    public async Task<JsonResult> OnOrderCreate() {
        // Construct a request object and set desired parameters
        // Here, OrdersCreateRequest() creates a POST request to /v2/checkout/orders
        var order = new OrderRequest {
            CheckoutPaymentIntent = "CAPTURE",
            PurchaseUnits = new List<PurchaseUnitRequest> {
                new() {
                    AmountWithBreakdown = new AmountWithBreakdown {
                        CurrencyCode = "USD",
                        Value = "100.00"
                    }
                }
            },
            ApplicationContext = new ApplicationContext {
                ReturnUrl =
                    $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/checkout/api/paypal/order/capture",
                CancelUrl = "https://www.example.com"
            }
        };

        // Call API with your client and get a response for your call
        var request = new OrdersCreateRequest();
        request.Prefer("return=representation");
        request.RequestBody(order);
        var response = await _paypalClient.Execute(request);
        var statusCode = response.StatusCode;
        var result = response.Result<Order>();

        Console.WriteLine("Status: {0}", result.Status);
        Console.WriteLine("Order Id: {0}", result.Id);
        Console.WriteLine("Intent: {0}", result.CheckoutPaymentIntent);
        Console.WriteLine("Links:");

        foreach ( var link in result.Links ) {
            Console.WriteLine("\t{0}: {1}\tCall Type: {2}", link.Rel, link.Href, link.Method);
        }

        return new JsonResult(result);
    }

    [HttpPost("capture/{id}")]
    public async Task<Order> OnOrderCapture(string id) {
        _logger.LogInformation("Capturing order id: {Id}", id);

        // Construct a request object and set desired parameters
        var request = new OrdersCaptureRequest(id);
        request.RequestBody(new OrderActionRequest());
        
        var response = await _paypalClient.Execute(request);
        var statusCode = response.StatusCode;
        var result = response.Result<Order>();

        _logger.LogInformation("Status: {0}", result.Status);
        _logger.LogInformation("Capture Id: {0}", result.Id);

        // if ( response.StatusCode != HttpStatusCode.OK ) {
        //     _logger.LogCritical("Error when trying to capture payment for id {Id}: Status Code {Code}", id,
        //         response.StatusCode);
        //     return Redirect("/");
        // }
        //
        // var user = await _userManager.GetUserByIdAsync(User.GetUniqueId());
        // if ( user == null ) {
        //     _logger.LogCritical("Critical error trying to get user information after capturing and processing PayPal payment");
        //     return Redirect("/");
        // }

        return result;
    }
}