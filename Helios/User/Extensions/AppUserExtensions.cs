using System.Security.Claims;
using Helios.Products;
using MongoDB.Bson;

namespace Helios.Data.Users.Extensions; 

public static class AppUserExtensions {

    public static ObjectId GetUniqueId(this ClaimsPrincipal principal) {
        var name = principal.FindFirst(ClaimTypes.NameIdentifier);
        return name == null ? ObjectId.Empty : new ObjectId(name.Value);
    }

    public static string GetDisplayName(this ClaimsPrincipal principal) {
        return principal.FindFirst(ClaimTypes.Name)?.Value;
    }

    public static bool IsLoggedIn(this ClaimsPrincipal principal) {
        return principal.Identity.IsAuthenticated;
    }

    public static IEnumerable<TimeSpan> GetEligibleNotifyTimespans(this ApplicationUser user) {
        var accountType = user.AccountType;

        //
        // Should return a list of all timespans under the minimum timespan allowed for each account type
        // e.g. Free tier should not get 5 minutes as their minimum timespan is 30 minutes
        // 

        return accountType switch {
            EAccountType.Free => NotifyTimespan.All.Where(x => x >= TimeSpan.FromMinutes(60)),
            EAccountType.Pro => NotifyTimespan.All.Where(x => x >= TimeSpan.FromMinutes(30)),
            EAccountType.Enterprise => NotifyTimespan.All.Where(x => x >= TimeSpan.FromMinutes(5)),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static string GenerateMemoForProduct(this ApplicationUser user, Product product) {
        var str = "";

        str += user.Id.ToString()[..7];
        str += product.Id;

        return str.Length > 8 ? null : str;
    }
}