using System.Web;
using Helios.MailService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Extensions; 

public static class MailSenderExtensions {
    public static async Task SendPasswordReset(this IMailSender sender, string email, string resetToken,
        PageModel context) {
        var callback = context.Url.Page("passwordreset", new {
            token = HttpUtility.UrlEncode(resetToken),
            email = email
        });

        await sender.SendEmailAsync(
            email,
            "Password reset",
            $"{context.Request.Scheme}://{context.Request.Host}{callback}",
            null
        );
    }
}