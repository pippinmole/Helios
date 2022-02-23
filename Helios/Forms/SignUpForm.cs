using System.ComponentModel.DataAnnotations;
using Helios.Attributes;

namespace Helios.Forms; 

public class SignUpForm {
    [Required, MaxLength(15), UnicodeOnly]
    public string Username { get; set; }
            
    [Required, DataType(DataType.EmailAddress), MaxLength(320), UnicodeOnly, EmailAddress]
    public string Email { get; set; }
            
    [Required, StringLength(32), DataType(DataType.Password), UnicodeOnly]
    public string Password { get; set; }

    [Required]
    public bool RememberMe { get; set; }
}