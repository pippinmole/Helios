using System.Web;
using FluentEmail.Core;
using FluentEmail.Razor;
using Helios.Email_Templates;
using Helios.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.MailService;

public class MailSender : IMailSender {

    private readonly ILogger<MailSender> _logger;
    private readonly IFluentEmailFactory _emailFactory;

    private string Root => Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates");

    public MailSender(ILogger<MailSender> logger, IFluentEmailFactory emailFactory) {
        _logger = logger;
        _emailFactory = emailFactory;
    }

    public async Task SendVerifyEmailAsync(string address, string username, string verifyUrl,
        CancellationToken? token = null) {
        var email = _emailFactory.Create()
            .To(address)
            .Subject("Verify your email")
            .UsingTemplateFromFile(Root + "/VerifyEmail.cshtml", new VerifyEmailModel {
                Username = username,
                VerifyUrl = verifyUrl
            });

        var response = await email.SendAsync(token).ConfigureAwait(false);
        if ( response.Successful ) {
            _logger.LogInformation("Successfully sent verify email: {Response}", response.Successful);
        } else {
            foreach ( var error in response.ErrorMessages ) {
                _logger.LogError("Failed to send email verification email: {Error}", error);
            }
        }
    }

    public async Task SendResetPasswordAsync(string address, string username, string resetUrl,
        CancellationToken? token = null) {
        
        var test = _emailFactory.Create();
        _logger.LogInformation("isValid: {IsValid}", test.Renderer.GetType() == typeof(RazorRenderer));
        
        var email = _emailFactory.Create()
            .To(address)
            .Subject("Password Reset")
            .UsingTemplateFromFile(Root + "/ResetPassword.cshtml", new ResetPasswordEmailModel {
                Username = username,
                ResetUrl = resetUrl
            });

        var response = await email.SendAsync(token).ConfigureAwait(false);
        if ( response.Successful ) {
            _logger.LogInformation("Successfully sent password reset email: {Response}", response.Successful);
        } else {
            foreach ( var error in response.ErrorMessages ) {
                _logger.LogError("Failed to send password reset email: {Response}", error);
            }
        }
    }

    public Task SendPurchaseConfirmedAsync(string address, Product order, CancellationToken? token = null) {
        return Task.CompletedTask;
    }

    public async Task SendServiceDownAsync(string address, CancellationToken? token = null) {
        var email = _emailFactory.Create()
            .To(address)
            .Subject("Hotspot is offline")
            .Body(
                "Attention! 1 or more of your hotspots are offline. Please check your dashboard for more information");

        var response = await email.SendAsync(token).ConfigureAwait(false);
        if ( response.Successful ) {
            _logger.LogInformation("Successfully sent service down email: {Response}", response.Successful);
        } else {
            foreach ( var error in response.ErrorMessages ) {
                _logger.LogError("Failed to send password reset email: {Response}", error);
            }
        }
    }
}