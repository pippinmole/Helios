using AspNetCoreHero.ToastNotification.Toastify;
using FluentEmail.Core;
using FluentEmail.Mailgun;
using Helios.Email_Templates;
using Helios.Extensions;

namespace Helios.MailService; 

public class MailSender : IMailSender  {

    private readonly ILogger<MailSender> _logger;
    private readonly IFluentEmailFactory _emailFactory;

    public MailSender(ILogger<MailSender> logger, IFluentEmailFactory emailFactory) {
        _logger = logger;
        _emailFactory = emailFactory;
    }

    public async Task SendVerifyEmail(string address, string username, string verifyUrl) {
        var email = _emailFactory.Create()
            .To(address)
            .Subject("Verify your email")
            .UsingTemplateFromFile("Email Templates/VerifyEmail.cshtml", new VerifyEmailModel {
                Username = username,
                VerifyUrl = verifyUrl
            });
        // .UsingMailgunTemplate("verify-email")
            // .UsingMailgunTemplateVariables(new Dictionary<string, string>() {
            //     { "username", username },
            //     { "verify_url", verifyUrl }
            // });

        _logger.LogInformation("To: {To}", email.Data.ToAddresses.ToList()[0].EmailAddress);
        _logger.LogInformation("FromAddress.Name: {Name}", email.Data.FromAddress.Name);
        _logger.LogInformation("FromAddress.EmailAddress: {Address}", email.Data.FromAddress.EmailAddress);
        _logger.LogInformation("FromAddress: {Address}", email.Data.FromAddress);

        var response = await email.SendAsync();
        
        _logger.LogInformation("Response Successful: {Response}", response.Successful);

        foreach ( var error in response.ErrorMessages ) {
            _logger.LogInformation("Response Error: {Response}", error);
        }
    }

    public async Task SendEmailAsync(string recipients, string subject, string body, string sender) {

        var email = Email
            .From("test@ruffles.pw")
            .To(recipients)
            .Subject(subject)
            .Body(body);

        await email.SendAsync();

        // var msg = new MailMessage {
        //     Subject = subject,
        //     Body = body,
        //     IsBodyHtml = true,
        //     From = new MailAddress(options.AddressFrom),
        //     BodyEncoding = Encoding.UTF8
        // };
        //
        // foreach ( var recipient in recipients ) {
        //     msg.To.Add(recipient);
        // }

        // Build SMTP Server
        // using var client = new SmtpClient {
        //     Host = options.SmtpHostAddress,
        //     Port = options.Port,
        //     EnableSsl = true,
        //     Credentials = new NetworkCredential(options.AddressFrom, options.Password)
        // };
        //
        // await client.SendMailAsync(msg);
    }
}