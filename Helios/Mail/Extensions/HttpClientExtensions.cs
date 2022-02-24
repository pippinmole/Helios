namespace Helios.Mail.Extensions;

public static class HttpClientExtensions {
    public static async Task<HttpResponseMessage> PostMultipart<T>(this HttpClient client, string url,
        IEnumerable<KeyValuePair<string, string>> parameters, CancellationToken cancellationToken = default) {
        return await client.PostAsync(url, GetMultipartFormDataContentBody(parameters), cancellationToken).ConfigureAwait(false);
    }

    private static HttpContent GetMultipartFormDataContentBody(IEnumerable<KeyValuePair<string, string>> parameters) {
        var mpContent = new MultipartFormDataContent();

        foreach ( var (key, value) in parameters ) {
            mpContent.Add(new StringContent(value), key);
        }

        return mpContent;
    }
}