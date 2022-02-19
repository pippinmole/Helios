using FluentEmail.Core;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.MailService;

public interface IMailSender {
    Task SendVerifyEmailAsync(string address, string username, string verifyUrl, CancellationToken? token = null);
    Task SendResetPasswordAsync(string address, string username, string resetUrl, CancellationToken? token = null);
}