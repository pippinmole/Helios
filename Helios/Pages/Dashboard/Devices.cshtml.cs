using AspNetCoreHero.ToastNotification.Abstractions;
using Helios.Data.Users;
using Helios.Data.Users.Extensions;
using Helios.Forms;
using Helios.Helium;
using Microsoft.AspNetCore.Mvc;

namespace Helios.Pages.Dashboard; 

public class DevicesModel : DashboardModel {
    private readonly ILogger<DevicesModel> _logger;
    private readonly IAppUserManager _userManager;
    private readonly IHeliumService _heliumService;
    private readonly INotyfService _notyfService;

    public DevicesModel(ILogger<DevicesModel> logger, IAppUserManager userManager, IHeliumService heliumService,
        INotyfService notyfService) : base(userManager) {
        _logger = logger;
        _userManager = userManager;
        _heliumService = heliumService;
        _notyfService = notyfService;
    }

    public async Task<IActionResult> OnPostAddDeviceAsync(string animalName) {
        if ( !ModelState.IsValid )
            return Page();

        var user = await _userManager.GetUserByIdAsync(User.GetUniqueId());
        if ( user == null ) return Redirect("/");

        if ( user.Devices.Count + 1 > user.AccountType.MaxDevicesAllowed() ) {
            _notyfService.Warning($"You cannot add any more devices. Please " +
                                  "<a href=\"/pricing\">upgrade your account</a>.");
            return Page();
        }
        
        if ( user.Devices.Any(x => x.AnimalName == animalName) ) {
            _notyfService.Warning("That device is already added");
            return Page();
        }

        var report = await _heliumService.GetHotspotByAnimalName(animalName);
        if ( report == null ) {
            // miner with provided name does not exist
            _notyfService.Error($"No miner with that name is registered on the Helium network");
            
            return Page();
        }
        
        user.Devices ??= new List<HeliumMiner>();
        user.Devices.Add(new HeliumMiner(animalName, report));

        _logger.LogInformation("Added device with name: {Name}", animalName);
        
        await _userManager.UpdateUserAsync(user);

        return Page();
    }
    
    public async Task<IActionResult> OnPostDeleteDeviceAsync(Guid id) {
        if ( !ModelState.IsValid )
            return Page();
        
        var user = await _userManager.GetUserByIdAsync(User.GetUniqueId());
        if ( user == null || !User.IsLoggedIn() )
            return Forbid();
        
        user.Devices ??= new List<HeliumMiner>();
        user.Devices.RemoveAll(x => x.Id == id);
        
        await _userManager.UpdateUserAsync(user);
        
        _logger.LogInformation("Successfully removed device");

        return Page();
    }
}