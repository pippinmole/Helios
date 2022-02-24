using System.Net.Http.Headers;
using System.Text;
using FluentEmail.Mailgun;
using Helios.Mail.Extensions;
using Helios.MailService;
using Newtonsoft.Json;

namespace Helios.Mail; 

public class Email {
    private readonly HttpClient _client;
    private readonly string _url;
    private readonly MailSenderOptions _options;

    private readonly EmailData _data = new();
    
    public Email(HttpClient client, string url, MailSenderOptions options) {
        _client = client;
        _url = url;
        _options = options;
        
        Authenticate();
        From();
    }

    private void Authenticate() {
        var auth = Encoding.ASCII.GetBytes($"api:{_options.ApiKey}");
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", Convert.ToBase64String(auth));
    }

    public Email To(string address) {
        // _data.ToAddress = address;
        _data.Parameters.Add(new KeyValuePair<string, string>("to", $"to <{address}>"));
        // _client.DefaultRequestHeaders.Add("to", address);
        return this;
    }
    
    public Email Subject(string subject) {
        _data.Parameters.Add(new KeyValuePair<string, string>("subject", subject));
        
        // _data.Subject = subject;
        // _client.DefaultRequestHeaders.Add("subject", subject);
        return this;
    }

    public Email WithTemplateVariables(string templateName, Dictionary<string, string> variables) {
        _data.Parameters.Add(new KeyValuePair<string, string>("template", templateName));
        _data.Parameters.Add(new("X-Mailgun-Variables", JsonConvert.SerializeObject(variables)));
        // _client.DefaultRequestHeaders.Add("X-Mailgun-Variables", JsonConvert.SerializeObject(variables));

        return this;
    }

    public Email Body(string body) {
        // _data.Body = body;
        _data.Parameters.Add(new KeyValuePair<string, string>("text", body));
        // _client.DefaultRequestHeaders.Add("body", body);
        return this;
    }
    
    private Email From() {
        // parameters.Add(new KeyValuePair<string, string>("from", $"{email.Data.FromAddress.Name} <{email.Data.FromAddress.EmailAddress}>"));
        
        _data.Parameters.Add(new KeyValuePair<string, string>("from", $"{_options.FromName} <{_options.FromName}@{_options.Domain}>"));
        // _data.FromAddress = address;
        // _client.DefaultRequestHeaders.Add("to", address);
        return this;
    }

    public Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken = default) {
        return _client.PostMultipart<HttpResponseMessage>(_url + "/messages", _data.Parameters, null, cancellationToken);
    }
}