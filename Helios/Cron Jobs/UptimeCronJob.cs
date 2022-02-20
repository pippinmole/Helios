using Helios.Data.Users;
using Helios.Helium;
using Helios.Helium.Schemas;
using Helios.MailService;

namespace Helios.Core;

public class UptimeCronJob : CronJobService {
    private readonly ILogger<UptimeCronJob> _logger;
    private readonly IHeliumService _heliumService;
    private readonly IServiceProvider _serviceProvider;

    public UptimeCronJob(IScheduleConfig<UptimeCronJob> config, ILogger<UptimeCronJob> logger,
        IHeliumService heliumService, IServiceProvider serviceProvider)
        : base(config.CronExpression, config.TimeZoneInfo) {

        _logger = logger;
        _heliumService = heliumService;
        _serviceProvider = serviceProvider;
    }

    public override async Task DoWork(CancellationToken cancellationToken) {
        var scope = _serviceProvider.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<IAppUserManager>();
        var mailService = scope.ServiceProvider.GetRequiredService<IMailSender>();

        // var users = userService.GetUsersWhere(x => x.Roles.Contains("Paid"));

        // TODO: Remove this
        // var users = userService.GetUsersInRoleAsync("");
        
        var users = userService.GetUsersWhere(x => true);

        foreach ( var user in users ) {
            // _logger.LogInformation("User has paid: {Username}", user.UserName);

            user.Devices ??= new List<HeliumMiner>();

            foreach ( var device in user.Devices ) {
                var report = await _heliumService.GetHotspotByAnimalName(device.AnimalName);
                device.UpdateReport(report);
            }

            if ( user.Devices.Any(x => !x.LastReport.status.IsOnline) ) {
                if ( !user.CanSendEmail() )
                    continue;

                _logger.LogInformation("Sending downtime email to {Username}", user.UserName);
                
                // Send downtime notification
                user.LastEmailDate = DateTime.Now;
                await mailService.SendServiceDownAsync(user.Email, cancellationToken);
            }

            await userService.UpdateUserAsync(user);
        }
    }
}