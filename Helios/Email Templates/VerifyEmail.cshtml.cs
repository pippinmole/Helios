using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Email_Templates; 

public class VerifyEmailModel : PageModel {
    
    [BindProperty] public string Username { get; set; }
    [BindProperty] public string VerifyUrl { get; set; }
    
}