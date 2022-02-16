using System.Security.Claims;
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
}