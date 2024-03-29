﻿using Helios.Helium.Helpers;
using Helios.Helium.Schemas;
using Newtonsoft.Json;

namespace Helios.Helium;

public class HeliumService : IHeliumService {

    public bool IsServiceDown => _serviceDownCount > 5;
    
    private readonly ILogger<HeliumService> _logger;
    private readonly HttpClient _client;

    private readonly JsonSerializerSettings _serializerSettings = new() {
        Error = (_, ev) => ev.ErrorContext.Handled = true
    };
    
    private int _serviceDownCount;

    public HeliumService(ILogger<HeliumService> logger) {
        _logger = logger;
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.29.0");
    }

    public async Task<HotspotReport> GetHotspotByAnimalName(string name) {
        if ( name == null ) return null;

        // fix name spaces
        name = name.Replace(" ", "-").ToLower();

        var url = $"https://api.helium.io/v1/hotspots/name/{name}";
        var result = await GetAsync<HotspotResult>(url);

        return result?.data == null || result.data.Count == 0
            ? null
            : result.data[0];
    }

    public async Task<(Block, Payment)> GetTransactionFromHash(string hash,
        CancellationToken cancellationToken = default) {
        if ( !HeliumHelper.IsValidHash(hash) )
            return (null, null);
        
        var url = $"https://api.helium.io/v1/transactions/{hash}";
        var result = await GetAsync<BlockRoot>(url, cancellationToken);

        return result?.data?.payments == null || result.data.payments.Count == 0
            ? (null, null)
            : (result.data, result.data.payments[0]);
    }

    private async Task<T> GetAsync<T>(string url, CancellationToken cancellationToken = default) where T : class {
        try {
            var json = await _client.GetStringAsync(url, cancellationToken);
            var obj = JsonConvert.DeserializeObject<T>(json, _serializerSettings);

            // Reset the count if we've had a successful request
            _serviceDownCount = 0;

            return obj;
        }
        catch ( HttpRequestException ex ) {
            _logger.LogError(ex, "Error {ErrorCode}: {ErrorMessage} when trying to access resource: {Url}",
                ex.StatusCode, ex.Message, url);

            _serviceDownCount++;
        }

        return null;
    }
}