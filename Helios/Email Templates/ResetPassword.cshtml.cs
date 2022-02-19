using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Email_Templates; 

public class ResetPasswordEmailModel : PageModel {
    public string Username { get; set; }
    public string ResetUrl { get; set; }
}