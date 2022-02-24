﻿using System.ComponentModel.DataAnnotations;
using System.Web;
using AspNetCoreHero.ToastNotification.Abstractions;
using Helios.Data.Users;
using Helios.MailService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Pages;

public class ForgotPasswordModel : PageModel {
    private readonly ILogger<ForgotPasswordModel> _logger;
    private readonly IMailSender _mailSender;
    private readonly IAppUserManager _userManager;
    private readonly INotyfService _notyfService;

    [BindProperty] public ForgotPasswordForm Form { get; set; }

    public ForgotPasswordModel(ILogger<ForgotPasswordModel> logger, IMailSender mailSender, IAppUserManager userManager, INotyfService notyfService) {
        _logger = logger;
        _mailSender = mailSender;
        _userManager = userManager;
        _notyfService = notyfService;
    }

    public async Task<IActionResult> OnPostAsync() {
        var form = Form;

        if ( string.IsNullOrEmpty(form.Email) ) return Page();
            
        var user = await _userManager.GetUserByEmailAsync(form.Email);
        if ( user == null ) {
            return Page();
        }

        var token = HttpUtility.UrlEncode(await _userManager.GeneratePasswordResetTokenAsync(user));
        var callback = Url.Page("ResetPassword", new {
            email = form.Email
        });

        await _mailSender.SendResetPasswordAsync(
            form.Email,
            user.UserName,
            $"{Request.Scheme}://{Request.Host}{Request.PathBase}{callback}?token={token}");

        _notyfService.Success("Please check your email to reset your password");
        
        return LocalRedirect("/");
    }

    public IActionResult OnGet(string email, string token) {
        return Page();
    }
        
    public class ForgotPasswordForm {
        [Required]
        public string Email { get; set; }
    }
}