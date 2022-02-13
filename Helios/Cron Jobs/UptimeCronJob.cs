using Helios.Data.Users;

namespace Helios.Core;

public class UptimeCronJob : CronJobService {
    private readonly ILogger<UptimeCronJob> _logger;
    private readonly IServiceProvider _serviceProvider;

    public UptimeCronJob(IScheduleConfig<UptimeCronJob> config, ILogger<UptimeCronJob> logger, IServiceProvider serviceProvider)
        : base(config.CronExpression, config.TimeZoneInfo) {
        
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public override Task DoWork(CancellationToken cancellationToken) {
        _logger.LogInformation("Doing work");
        
        var scope = _serviceProvider.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<IAppUserManager>();

        var users = userService.GetUsersWhere(x => x.Roles.Contains("Paid"));
        foreach ( var user in users ) {
            _logger.LogInformation("User has paid: {Username}", user.Username);
        }
        
        return base.DoWork(cancellationToken);
    }
}