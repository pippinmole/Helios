using System.ComponentModel.DataAnnotations;
using Helios.Attributes;

namespace Helios.Forms; 

public class ChangeAccountForm {
    [MaxLength(15)]
    public string Username { get; set; }
    [DataType(DataType.EmailAddress), MaxLength(320), UnicodeOnly]
    public string Email { get; set; }
    [StringLength(32), DataType(DataType.Password)]
    public string CurrentPassword { get; set; }
    [StringLength(32), DataType(DataType.Password)]
    public string NewPassword { get; set; }
    public TimeSpan NewNotifyTimespan { get; set; }
    public bool ReceiveEmails { get; set; }
}
