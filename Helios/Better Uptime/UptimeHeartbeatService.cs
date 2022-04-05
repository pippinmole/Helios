using Helios.Better_Uptime.Options;
using Microsoft.Extensions.Options;

namespace Helios.Better_Uptime; 

public class UptimeHeartbeatService : IUptimeHeartbeatService {
    private readonly ILogger<UptimeHeartbeatService> _logger;
    private readonly IOptions<UptimeHeartbeatOptions> _options;
    private readonly HttpClient _client = new();

    public UptimeHeartbeatService(ILogger<UptimeHeartbeatService> logger, IOptions<UptimeHeartbeatOptions> options) {
        _logger = logger;
        _options = options;
    }
    
    public async Task DeviceCheckHeartbeatAsync() {
        
        var url = _options?.Value.EmailHeartbeatUrl;
        if ( string.IsNullOrEmpty(url) ) return;

        var response = await _client.GetAsync(url);
        if ( !response.IsSuccessStatusCode ) {
            _logger.LogWarning("Non-success status code ({StatusCode}) when trying to send email heartbeat", response.StatusCode);
        } else {
            _logger.LogInformation("Successfully sent heartbeat to BetterUptime API");
        }
    }
}

public interface IUptimeHeartbeatService {
    Task DeviceCheckHeartbeatAsync();
}