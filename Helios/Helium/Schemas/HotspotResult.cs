using System.Net;

namespace Helios.Helium.Schemas;

public class Status {
    public DateTime timestamp { get; set; }
    public string online { get; set; }
    public List<string> listen_addrs { get; set; }
    public int height { get; set; }

    public IEnumerable<string> GetListenAddresses() {
        return listen_addrs
            .Select(ip => ip.Remove(0, 5))
            .Select(address => address.Remove(address.IndexOf("/tcp/", StringComparison.Ordinal)));
    }
}

public class Geocode {
    public string short_street { get; set; }
    public string short_state { get; set; }
    public string short_country { get; set; }
    public string short_city { get; set; }
    public string long_street { get; set; }
    public string long_state { get; set; }
    public string long_country { get; set; }
    public string long_city { get; set; }
    public string city_id { get; set; }
}

public class HotspotReport {
    public double lng { get; set; }
    public double lat { get; set; }
    public DateTime timestamp_added { get; set; }
    public Status status { get; set; }
    public double reward_scale { get; set; }
    public string payer { get; set; }
    public string owner { get; set; }
    public int nonce { get; set; }
    public string name { get; set; }
    public string mode { get; set; }
    public string location_hex { get; set; }
    public string location { get; set; }
    public int last_poc_challenge { get; set; }
    public int last_change_block { get; set; }
    public Geocode geocode { get; set; }
    public int gain { get; set; }
    public int elevation { get; set; }
    public int block_added { get; set; }
    public int block { get; set; }
    public string address { get; set; }
}

public class HotspotResult {
    public List<HotspotReport> data { get; set; }
}
