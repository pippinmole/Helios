using Helios.Data.Users;
using Helios.Model_Templates;

namespace Helios.Pages.Dashboard; 

public class DashboardModel : AuthorizedPageModel {
    
    public DashboardModel(IAppUserManager userManager) : base(userManager) { }
    
}