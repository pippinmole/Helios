using System.ComponentModel.DataAnnotations;

namespace Helios.Forms; 

public class ChangeAccountForm {
    public string Username { get; set; }
    public string Email { get; set; }
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
    public TimeSpan NewNotifyTimespan { get; set; }
    public bool ReceiveEmails { get; set; }
}
