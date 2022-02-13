using System.Collections.Generic;
using System.Threading.Tasks;

namespace Helios.MailService; 

public interface IMailSender {

    Task SendEmailAsync(IEnumerable<string> recipients, string subject, string body, string sender);

}