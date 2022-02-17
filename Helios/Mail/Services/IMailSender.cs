using System.Collections.Generic;
using System.Threading.Tasks;

namespace Helios.MailService; 

public interface IMailSender {

    Task SendEmailAsync(string recipients, string subject, string body, string sender);
    Task SendVerifyEmail(string address, string username, string verifyUrl);

}