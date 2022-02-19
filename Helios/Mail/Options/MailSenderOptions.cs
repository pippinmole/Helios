namespace Helios.MailService; 

public class MailSenderOptions {
        
    public const string Name = "MailSenderOptions";

    public string FromName { get; set; }
    public string ApiKey { get; set; }
    public string Domain { get; set; }
}