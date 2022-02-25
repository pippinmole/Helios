using System.ComponentModel.DataAnnotations;

namespace Helios.Forms; 

public class ForgotPasswordForm {
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}