using Helios.Helium.Schemas;
using Newtonsoft.Json;

namespace Helios.Helium; 

public class HeliumService : IHeliumService {

    private readonly HttpClient _client;
    private readonly JsonSerializerSettings _serializerSettings = new() {
        Error = (_, ev) => ev.ErrorContext.Handled = true
    };

    public HeliumService() {
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.29.0");
    }
    
    public async Task<Hotspot?> GetHotspotByAnimalName(string name) {
        var url = $"https://api.helium.io/v1/hotspots/name/{name}";
        var result = await Get<HotspotResult>(url);

        return result.data.Count == 0
            ? null
            : result.data[0];
    }

    private async Task<T> Get<T>(string url) {
        var json = await _client.GetStringAsync(url);
        var obj = JsonConvert.DeserializeObject<T>(json, _serializerSettings);
            
        return obj;
    }
}