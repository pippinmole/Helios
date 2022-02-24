using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Pages; 

public class ResetPasswordEmailModel : PageModel {
    public string Username { get; set; }
    public string ResetUrl { get; set; }
}