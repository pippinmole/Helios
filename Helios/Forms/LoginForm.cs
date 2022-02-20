using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;

namespace Helios.Forms; 

public class LoginForm {
    [Required, MaxLength(15)] public string Username { get; set; }

    [Required, StringLength(32), DataType(DataType.Password)]
    public string Password { get; set; }
        
    public bool RememberMe { get; set; }
}