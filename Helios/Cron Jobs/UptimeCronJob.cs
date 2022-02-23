using Helios.Data.Users;
using Helios.Helium;
using Helios.MailService;
using Microsoft.Extensions.Options;

namespace Helios.Core;

public class UptimeCronJob : CronJobService {
    private readonly ILogger<UptimeCronJob> _logger;
    private readonly IHeliumService _heliumService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<HeliumOptions> _heliumOptions;

    public UptimeCronJob(IScheduleConfig<UptimeCronJob> config, ILogger<UptimeCronJob> logger,
        IHeliumService heliumService, IServiceProvider serviceProvider, IOptions<HeliumOptions> heliumOptions)
        : base(config.CronExpression, config.TimeZoneInfo) {

        _logger = logger;
        _heliumService = heliumService;
        _serviceProvider = serviceProvider;
        _heliumOptions = heliumOptions;
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
            await UpdateDeviceData(user, mailService, cancellationToken);

            await userService.UpdateUserAsync(user);
        }
    }

    private async Task UpdateDeviceData(ApplicationUser user, IMailSender mailSender, CancellationToken cancellationToken) {
        user.Devices ??= new List<HeliumMiner>();

        foreach ( var device in user.Devices ) {
            var report = await _heliumService.GetHotspotByAnimalName(device.AnimalName);
            device.UpdateReport(report);
        }

        if ( user.Devices.Any(x => x != null && !x.LastReport.status.IsOnline) ) {
            if ( !user.CanSendEmail() ) return;

            _logger.LogInformation("Sending downtime email to {Username}", user.UserName);

            // Send downtime notification
            user.LastEmailDate = DateTime.Now;
            await mailSender.SendServiceDownAsync(user.Email, cancellationToken);
        }
    }
}