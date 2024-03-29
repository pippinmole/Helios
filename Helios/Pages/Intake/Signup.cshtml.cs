﻿using System.Web;
using Helios.Data.Users;
using Helios.Forms;
using Helios.MailService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Pages;

public class SignupModel : PageModel {
    private readonly ILogger<SignupModel> _logger;
    private readonly IAppUserManager _userManager;
    private readonly IMailSender _mailSender;

    [BindProperty] public SignUpForm SignupForm { get; set; }

    public SignupModel(ILogger<SignupModel> logger, IAppUserManager userManager, IMailSender mailSender) {
        _logger = logger;
        _userManager = userManager;
        _mailSender = mailSender;
    }

    public async Task<IActionResult> OnPost() {
        if ( !ModelState.IsValid )
            return Page();

        _logger.LogInformation("Signing up user {Username}", SignupForm.Username);

        var user = new ApplicationUser(SignupForm.Username, SignupForm.Email);
        var result = await _userManager.CreateAsync(user, SignupForm.Password);

        _logger.LogInformation("Created user {Username}", SignupForm.Username);

        if ( result.Succeeded ) {
            var token = HttpUtility.UrlEncode(await _userManager.GenerateEmailConfirmTokenAsync(user));
            var callback = Url.Page("SignupVerification", new {
                email = user.Email
            });
            
            var url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}{callback}?token={token}";
            
            _logger.LogInformation("Url is {Url}", url);

            await _mailSender.SendVerifyEmailAsync(user.Email, user.UserName, url);
            await _userManager.SignInAsync(user, SignupForm.RememberMe);
            
            return Redirect("/");
        }

        foreach ( var error in result.Errors ) {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return Page();
    }
}