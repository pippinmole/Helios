﻿using Helios.Better_Uptime;
using Helios.Data.Users;
using Helios.Data.Users.Extensions;
using Helios.Helium;
using Helios.MailService;
using Microsoft.Extensions.Options;

namespace Helios.Core;

public class UptimeCronJob : CronJobService {
    private readonly ILogger<UptimeCronJob> _logger;
    private readonly IHeliumService _heliumService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<HeliumOptions> _heliumOptions;
    private readonly IUptimeHeartbeatService _uptimeHeartbeatService;

    public UptimeCronJob(IScheduleConfig<UptimeCronJob> config, ILogger<UptimeCronJob> logger,
        IHeliumService heliumService, IServiceProvider serviceProvider, IOptions<HeliumOptions> heliumOptions,
        IUptimeHeartbeatService uptimeHeartbeatService)
        : base(config.CronExpression, config.TimeZoneInfo) {

        _logger = logger;
        _heliumService = heliumService;
        _serviceProvider = serviceProvider;
        _heliumOptions = heliumOptions;
        _uptimeHeartbeatService = uptimeHeartbeatService;
    }

    public override async Task DoWorkAsync(CancellationToken cancellationToken) {
        var scope = _serviceProvider.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<IAppUserManager>();
        var mailService = scope.ServiceProvider.GetRequiredService<IMailSender>();
        
        var users = userService.GetUsersWhere(x => true);
        foreach ( var user in users ) {
            if ( !user.CanUpdateDevices() ) continue;
            
            await UpdateDeviceData(user, mailService, cancellationToken);
            await userService.UpdateUserAsync(user);
        }

        await _uptimeHeartbeatService.DeviceCheckHeartbeatAsync();
        _logger.LogInformation("Successfully completed {Name} job", nameof(UptimeCronJob));
    }

    private async Task UpdateDeviceData(ApplicationUser user, IMailSender mailSender,
        CancellationToken cancellationToken) {

        user.Devices ??= new List<HeliumMiner>();
        user.LastDeviceUpdate = DateTime.Now;

        foreach ( var device in user.Devices ) {
            var report = await _heliumService.GetHotspotByAnimalName(device.AnimalName);
            
            device.UpdateReport(report);
        }

        if ( user.Devices.Any(x => x != null && x.LastReport != null && !x.LastReport.status.IsOnline) ) {
            if ( !user.CanSendEmail() ) return;

            // Send downtime notification
            user.LastEmailDate = DateTime.Now;
            await mailSender.SendServiceDownAsync(user.Email, cancellationToken);
        }
    }
}