using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helios.Pages; 

public class VerifyEmailModel : PageModel {
    
    [BindProperty] public string Username { get; set; }
    [BindProperty] public string VerifyUrl { get; set; }
    
}