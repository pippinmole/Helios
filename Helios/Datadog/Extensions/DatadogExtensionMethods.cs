using Serilog;
using Serilog.Events;

namespace Helios.Datadog.Extensions; 

public static class DatadogExtensionMethods {

    public static IHostBuilder SetupDatadogLogging(this ConfigureHostBuilder builder) {
        builder.UseSerilog((context, config) => {
            config.ReadFrom.Configuration(context.Configuration);

            var options = context.Configuration.GetSection("Serilog:Datadog")?.Get<DataDogOptions>();
            if ( context.HostingEnvironment.IsDevelopment() || string.IsNullOrEmpty(options?.ApiKey) ) return;

            config.WriteTo.DatadogLogs(
                options.ApiKey,
                ".NET",
                options.ServiceName ?? "Helios",
                options.HostName ?? Environment.MachineName,
                new[] {
                    $"env:{options.EnvironmentName ?? context.HostingEnvironment.EnvironmentName}",
                    $"assembly:{options.AssemblyName ?? context.HostingEnvironment.ApplicationName}"
                },
                options.ToDatadogConfiguration(),
                logLevel: options.OverrideLogLevel ?? LogEventLevel.Verbose
            );
        });

        return builder;
    }
}