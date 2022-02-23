using Serilog.Events;
using Serilog.Sinks.Datadog.Logs;

namespace Helios.Datadog; 

public class DataDogOptions {
    public string? ApiKey { get; set; }
    public string ServiceName { get; set; }
    public string HostName { get; set; }
    public string EnvironmentName { get; set; }
    public string AssemblyName { get; set; }
    public LogEventLevel? OverrideLogLevel { get; set; } = LogEventLevel.Fatal;
    
    public string URL { get; private set; } = "intake.logs.datadoghq.com";
    public int Port { get; private set; } = 10516;
    public bool UseSSL { get; private set; } = true;
    public bool UseTCP { get; private set; } = true;

    public DatadogConfiguration ToDatadogConfiguration() => new(URL, Port, UseSSL, UseTCP);
}