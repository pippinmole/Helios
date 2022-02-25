using System.Net.Http.Headers;
using System.Text;
using Helios.Mail.Extensions;
using Helios.MailService;

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
        _data.Parameters.Add(new KeyValuePair<string, string>("to", $"to <{address}>"));
        return this;
    }
    
    public Email Subject(string subject) {
        _data.Parameters.Add(new KeyValuePair<string, string>("subject", subject));
        return this;
    }

    public Email WithTemplateVariables(string templateName, Dictionary<string, string> variables) {
        _data.Parameters.Add(new KeyValuePair<string, string>("template", templateName));

        foreach ( var (varKey, varValue) in variables ) {
            _data.Parameters.Add(new KeyValuePair<string, string>($"v:{varKey}", varValue));
        }
        
        return this;
    }

    public Email Body(string body) {
        _data.Parameters.Add(new KeyValuePair<string, string>("text", body));
        return this;
    }
    
    private Email From() {
        _data.Parameters.Add(new KeyValuePair<string, string>("from", $"{_options.FromName} <{_options.FromName}@{_options.Domain}>"));
        return this;
    }

    public Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken = default) {
        return _client.PostMultipart<HttpResponseMessage>(_url + "/messages", _data.Parameters, cancellationToken);
    }
}