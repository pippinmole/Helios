
using FluentEmail.Core;
using FluentEmail.Mailgun.HttpHelpers;

namespace Helios.Mail.Extensions;

public static class HttpClientExtensions {
    public static async Task<HttpResponseMessage> PostMultipart<T>(this HttpClient client, string url,
        IEnumerable<KeyValuePair<string, string>> parameters, IEnumerable<HttpFile> files, CancellationToken cancellationToken = default) {
        return await client.PostAsync(url, GetMultipartFormDataContentBody(parameters, files), cancellationToken).ConfigureAwait(false);
    }

    private static HttpContent GetMultipartFormDataContentBody(IEnumerable<KeyValuePair<string, string>> parameters,
        IEnumerable<HttpFile> files) {
        var mpContent = new MultipartFormDataContent();

        parameters?.ForEach(p => {
            var (key, value) = p;
            mpContent.Add(new StringContent(value), key);
        });

        files?.ForEach(file => {
            using var memoryStream = new MemoryStream();
            file.Data.CopyTo(memoryStream);
            mpContent.Add(new ByteArrayContent(memoryStream.ToArray()), file.ParameterName, file.Filename);
        });

        return mpContent;
    }
}