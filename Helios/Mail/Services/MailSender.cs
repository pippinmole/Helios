using Helios.Mail;
using Helios.Products;
using Microsoft.Extensions.Options;

namespace Helios.MailService;

public class MailSender : IMailSender {

    private readonly ILogger<MailSender> _logger;
    private readonly IOptions<MailSenderOptions> _options;
    private readonly IHttpClientFactory _httpClientFactory;

    private string BaseUrl => $"https://api.eu.mailgun.net/v3/{_options.Value.Domain}";
    
    public MailSender(ILogger<MailSender> logger, IOptions<MailSenderOptions> options, IHttpClientFactory httpClientFactory) {
        _logger = logger;
        _options = options;
        _httpClientFactory = httpClientFactory;
    }

    public Email Create() {
        var client = _httpClientFactory.CreateClient();

        return new Email(client, BaseUrl, _options.Value);
    }

    public async Task SendVerifyEmailAsync(string address, string username, string verifyUrl, CancellationToken token = default) {

        var email = Create()
            .To(address)
            .Subject("Verify your email")
            .WithTemplateVariables("verify-email", new Dictionary<string, string> {
                { "username", username },
                { "verify_url", verifyUrl }
            });
        
        var response = await email.SendAsync(token).ConfigureAwait(false);
        if ( response.IsSuccessStatusCode ) {
            _logger.LogInformation("Successfully sent verify email");
        } else {
            _logger.LogError("Failed to send email verification email: Error code {ErrorCode}", response.StatusCode);
        }
    }

    public async Task SendResetPasswordAsync(string address, string username, string resetUrl,
        CancellationToken token = default) {
        var email = Create()
            .To(address)
            .Subject("Password Reset")
            .WithTemplateVariables("reset-password", new Dictionary<string, string> {
                { "username", username },
                { "reset_url", resetUrl }
            });

        var response = await email.SendAsync(token).ConfigureAwait(false);
        if ( response.IsSuccessStatusCode ) {
            _logger.LogInformation("Successfully sent password reset email");
        } else {
            _logger.LogError("Failed to send password reset email: Error code {ErrorCode}", response.StatusCode);
        }
    }

    public Task SendPurchaseConfirmedAsync(string address, Product order, CancellationToken token = default) {
        return Task.CompletedTask;
    }

    public async Task SendServiceDownAsync(string address, CancellationToken token = default) {
        var email = Create()
            .To(address)
            .Subject("Hotspot is offline")
            .Body(
                "Attention! 1 or more of your hotspots are offline. Please check your dashboard for more information");

        var response = await email.SendAsync(token).ConfigureAwait(false);
        if ( response.IsSuccessStatusCode ) {
            _logger.LogInformation("Successfully sent service down email");
        } else {
            _logger.LogError("Failed to send service down email: Error code {ErrorCode}", response.StatusCode);
        }
    }
}