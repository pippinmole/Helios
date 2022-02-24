using Helios.Mail;
using Helios.Products;

namespace Helios.MailService;

public interface IMailSender {
    Email Create();
    
    Task SendVerifyEmailAsync(string address, string username, string verifyUrl, CancellationToken token = default);
    Task SendResetPasswordAsync(string address, string username, string resetUrl, CancellationToken token = default);
    Task SendPurchaseConfirmedAsync(string address, Product order, CancellationToken token = default);
    Task SendServiceDownAsync(string address, CancellationToken token = default);
}